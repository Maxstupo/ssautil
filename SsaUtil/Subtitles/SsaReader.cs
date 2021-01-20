namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;

    public enum SsaSection : int {
        None = -1,
        Unknown = 0,
        Info = 1,
        Styles = 2,
        Events = 3,
    }

    public sealed class SsaReader : BaseSsaReader<SsaSubtitle<SsaStyle, SsaEvent>, SsaStyle, SsaEvent> { }

    public class BaseSsaReader<T, S, E> : IEnumerable<SsaError> where T : SsaSubtitle<S, E> where S : SsaStyle where E : SsaEvent {

        protected string[] PropertySeparator { get; set; } = { ":" };
        protected string[] CsvSeparator { get; set; } = { "," };

        /// <summary>The current CSV column names to column indices mapping.</summary>
        protected Dictionary<string, int> ColumnNames { get; } = new Dictionary<string, int>();

        /// <summary>The section we are currently parsing. Defined in file by square brackets e.g. [Script Info]</summary>
        protected SsaSection CurrentSection { get; set; }

        /// <summary>The current subtitle object being parsed into.</summary>
        protected T Subtitle { get; set; }

        /// <summary>Errors discovered during the last subtitle file parsed.</summary>
        public Stack<SsaError> Errors { get; } = new Stack<SsaError>();

        /// <summary>True if the Error stack has items.</summary>
        public bool HasErrors => Errors.Count > 0;

        private Dictionary<Type, Dictionary<string, PropertyInfo>> PropertyInfoCache { get; } = new Dictionary<Type, Dictionary<string, PropertyInfo>>();


        public BaseSsaReader() {
            PropertyInfoCache.Add(typeof(T), SsaPropertyAttribute.GetSsaProperties(typeof(T)));
        }

        public T ReadFrom(string filepath) {
            return ReadFrom(filepath, Encoding.UTF8);
        }

        public T ReadFrom(string filepath, Encoding encoding) {
            Errors.Clear();

            if (!File.Exists(filepath)) {
                PushError(0, "File not found: {0}", filepath);
                return null;
            }

            Subtitle = Activator.CreateInstance<T>();

            using (StreamReader sr = new StreamReader(filepath, encoding)) {

                int lineNumber = 0;
                string line;
                while ((line = sr.ReadLine()) != null) {
                    lineNumber++;

                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string trimmedLine = line.Trim();

                    if (trimmedLine.StartsWith(";", StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    SsaSection section = DetermineSection(trimmedLine);
                    if (section != SsaSection.None) {
                        CurrentSection = section;
                        ColumnNames.Clear(); // Clear column names whenever the section changes, as the CSV headers might have changed.
                        continue;
                    }

                    ParseLine(lineNumber, line);


                }

            }
            return Subtitle;
        }


        /// <summary>
        /// Parses the provided <paramref name="line"/>. Called for each line in the subtitle file. Use <see cref="CurrentSection"/> to identify current SSA section.
        /// </summary>
        protected virtual void ParseLine(int lineNumber, string line) {
            switch (CurrentSection) {
                case SsaSection.Info:
                    ParseInfo(lineNumber, line);
                    break;

                case SsaSection.Styles:
                    S style = ParseCsv<S>(lineNumber, line, out _);

                    if (style != null)
                        Subtitle.Styles.Add(style.Name, style);
                    break;

                case SsaSection.Events:
                    E evt = ParseCsv<E>(lineNumber, line, out string type);

                    if (evt != null) {
                        evt.IsComment = type.Equals("Comment", StringComparison.InvariantCultureIgnoreCase);
                        Subtitle.Events.Add(evt);
                    }
                    break;

                default:
                    break;
            }
        }

        protected void ParseInfo(int lineNumber, string line) {
            string[] tokens = line.Split(PropertySeparator, 2, StringSplitOptions.None);

            // Access PropertyInfoCache directly as entry for typeof(T) always exists - added in ctor
            if (PropertyInfoCache[typeof(T)].TryGetValue(tokens[0], out PropertyInfo propertyInfo)) {
                string value = tokens[1].TrimStart();

                if (!TrySetPropertyValue(Subtitle, propertyInfo, value))
                    PushError(lineNumber, "Unable to parse {0} as {1} for value {2} in section {3} @ line {4}", tokens[0], propertyInfo.PropertyType, value, CurrentSection, lineNumber);

            } else {
                PushError(lineNumber, "Unknown property name {0} in section {1} @ line {2}", tokens[0], CurrentSection, lineNumber);
            }

        }

        protected R ParseCsv<R>(int lineNumber, string line, out string type) where R : class {

            string[] tokens = line.Split(PropertySeparator, 2, StringSplitOptions.None);
            type = tokens[0];

            string value = tokens[1].TrimStart();
            string[] rowValues = (ColumnNames.Count == 0) ? // If column names are defined, limit # of columns split.
                value.Split(CsvSeparator, StringSplitOptions.None)
                :
                value.Split(CsvSeparator, ColumnNames.Count, StringSplitOptions.None);

            if (tokens[0].Equals("Format", StringComparison.InvariantCultureIgnoreCase)) {
                ColumnNames.Clear();

                for (int i = 0; i < rowValues.Length; i++)
                    ColumnNames[rowValues[i].TrimStart()] = i;
            } else {

                if (ColumnNames.Count == 0) {
                    PushError(lineNumber, "Format undefined for csv row in section {0} @ line {1}", CurrentSection, lineNumber);
                    return null;
                }
                R rowObj = Activator.CreateInstance<R>();

                if (!PropertyInfoCache.TryGetValue(typeof(R), out Dictionary<string, PropertyInfo> properties)) {
                    properties = SsaPropertyAttribute.GetSsaProperties(typeof(R));
                    PropertyInfoCache.Add(typeof(R), properties);
                }

                foreach (KeyValuePair<string, int> column in ColumnNames) {

                    if (properties.TryGetValue(column.Key, out PropertyInfo propertyInfo)) {
                        string rowColumnValue = rowValues[column.Value];

                        if (!TrySetPropertyValue(rowObj, propertyInfo, rowColumnValue))
                            PushError(lineNumber, "Unable to parse {0} as {1} for value {2} in section {3} @ line {4}", column.Key, propertyInfo.PropertyType, rowColumnValue, CurrentSection, lineNumber);

                    } else {
                        PushError(lineNumber, "Unknown property name {0} in section {1} @ line {2}", column.Key, CurrentSection, lineNumber);

                    }

                }

                return rowObj;

            }

            return null;
        }


        /// <summary>
        /// Returns the <see cref="SsaSection"/> based on the provided <paramref name="line"/> (e.g. Events or V4+ Styles), or <see cref="SsaSection.None"/> if the provided line didn't match a section header.
        /// </summary>
        /// <param name="line">A line of text from the subtitle file.</param>
        /// <returns>A <see cref="SsaSection"/> or <see cref="SsaSection.None"/> if the <paramref name="line"/> isn't a section header.</returns>
        protected virtual SsaSection DetermineSection(string line) {
            if (line.Equals("[Script Info]", StringComparison.InvariantCultureIgnoreCase)) {
                return SsaSection.Info;

            } else if (line.Equals("[V4+ Styles]", StringComparison.InvariantCultureIgnoreCase) || line.Equals("[V4 Styles]", StringComparison.InvariantCultureIgnoreCase)) {
                return SsaSection.Styles;

            } else if (line.Equals("[Events]", StringComparison.InvariantCultureIgnoreCase)) {
                return SsaSection.Events;

            } else if (line[0] == '[' && line[line.Length - 1] == ']') {
                return SsaSection.Unknown;

            }

            return SsaSection.None;
        }

        /// <inheritdoc cref="TrySetProperty(object, PropertyInfo, string)"/>
        protected virtual bool TrySetPropertyValue(object obj, PropertyInfo propertyInfo, string value) {
            return TrySetProperty(obj, propertyInfo, value);
        }

        /// <summary>
        /// Attempts to set a property using a string representation of its value. Returns true if property was set, or false if the string representation couldn't be converted.
        /// </summary>
        /// <param name="obj">The object to set the property </param>
        /// <param name="propertyInfo">The property we are attempting to set.</param>
        /// <param name="value">The text representation of the value.</param>
        /// <returns>True if the property value was set.</returns>
        public static bool TrySetProperty(object obj, PropertyInfo propertyInfo, string value) {
            if (!typeof(string).IsAssignableFrom(propertyInfo.PropertyType)) {

                switch (Type.GetTypeCode(propertyInfo.PropertyType)) {
                    case TypeCode.Int32:
                        if (!int.TryParse(value, out int resultInt))
                            return false;
                        propertyInfo.SetValue(obj, resultInt);
                        break;

                    case TypeCode.Single:
                        if (!float.TryParse(value, out float resultSingle))
                            return false;
                        propertyInfo.SetValue(obj, resultSingle);
                        break;

                    case TypeCode.Boolean:
                        if (!int.TryParse(value, out int boolValue))
                            return false;
                        propertyInfo.SetValue(obj, boolValue != 0);
                        break;

                    default:
                        return false;

                }

            } else {
                propertyInfo.SetValue(obj, value);
            }

            return true;
        }

        /// <summary>Push an error onto the error stack. Convenience method.</summary>
        protected void PushError(int lineNumber, string error, params object[] args) {
            Errors.Push(new SsaError(error, lineNumber, args));
        }

        public IEnumerator<SsaError> GetEnumerator() {
            return Errors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }

}