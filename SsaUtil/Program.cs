namespace Maxstupo.SsaUtil {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using CommandLine;
    using Maxstupo.SsaUtil.Subtitles;
    using Maxstupo.SsaUtil.Utility;

    public sealed class Program {

        private IOutput Output { get; } = new ColorConsole();

        private readonly SsaReader reader = new SsaReader();
        private readonly SsaWriter writer = new SsaWriter();

        private HashSet<string> inputFiles;

        private Type scopeType;
        private Dictionary<string, PropertyInfo> properties;

        private readonly List<Setter> setters = new List<Setter>();
        private readonly List<Filter> filters = new List<Filter>();


        private int? Init(BaseOptions options) {

            inputFiles = options.Inputs.SelectMany(filepath => {
                if (Directory.Exists(filepath)) {
                    return Directory.EnumerateFiles(filepath, "*.ass", options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

                } else {
                    return Enumerable.Repeat(filepath, 1);

                }
            }).ToHashSet();

            if (inputFiles.Count == 0) {
                Output.WriteLine(Level.Severe, "No input files specified! Use the &-a;-i&-^; option.");
                return -1;
            }

            scopeType = options.ScopeStyles ? typeof(SsaStyle) : options.ScopeEvents ? typeof(SsaEvent) : typeof(SsaSubtitle<SsaStyle, SsaEvent>);
            properties = SsaPropertyAttribute.GetSsaProperties(scopeType);

            return null;
        }

        private int RunEdit(EditOptions options) {

            // Build setter list & validate property name existence.
            foreach (string setter in options.Setters) {
                string[] tokens = setter.Split(new string[] { "=" }, 2, StringSplitOptions.None);

                if (properties.ContainsKey(tokens[0])) {
                    setters.Add(new Setter(tokens[0], tokens[1]));

                } else {
                    Output.WriteLine(Level.Severe, $"Setter: Unknown property: &-e;{tokens[0]}&-^;");
                    return -1;
                }
            }

            // Build filter list & validate property name existence.
            foreach (string filter in options.Filters) {
                string[] tokens = filter.Split(new string[] { "=", "<", ">" }, 2, StringSplitOptions.None);

                if (properties.ContainsKey(tokens[0])) {

                    string[] values = tokens[1].Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                    Operator oper = filter.Contains("=") ? Operator.Equals : filter.Contains("<") ? Operator.LessThan : Operator.GreaterThan;


                    filters.Add(new Filter(tokens[0], oper, values));

                } else {
                    Output.WriteLine(Level.Severe, $"Filter: Unknown property: &-e;{tokens[0]}&-^;");
                    return -1;
                }
            }


            foreach (string file in inputFiles) {
                if (!File.Exists(file)) {
                    Output.WriteLine(Level.Warn, $"File doesn't exist &-3;{file}&-^; Ignoring...");
                    continue;
                }

                Output.WriteLine(Level.None, $"Parsing: &-3;{file}&-^;");


                SsaSubtitle<SsaStyle, SsaEvent> subtitle = reader.ReadFrom(file);
                bool hasReaderErrors = CheckReaderErrors();
                if (hasReaderErrors && !options.IgnoreErrors)
                    return -1;

                // Setters
                if (setters.Count > 0) {
                    if (!options.ScopeEvents && !options.ScopeStyles) { // Setter: Info
                        if (!IsFiltered(subtitle, out bool hadError)) {
                            if (hadError)
                                return -1;

                            if (!TryApplySetters(subtitle))
                                return -1;
                        }

                    } else if (options.ScopeStyles) {
                        foreach (SsaStyle style in subtitle.Styles.Values) { // Setter: Styles
                            if (IsFiltered(style, out bool hadError))
                                continue;
                            if (hadError)
                                return -1;

                            if (!TryApplySetters(style))
                                return -1;
                        }
                    } else {
                        foreach (SsaEvent evt in subtitle.Events) { // Setter: Events
                            if (IsFiltered(evt, out bool hadError))
                                continue;
                            if (hadError)
                                return -1;

                            if (!TryApplySetters(evt))
                                return -1;
                        }
                    }
                }

                Output.WriteLine(Level.Info, $"  - Title: &-e;{subtitle.Title}&-^;");
                Output.WriteLine(Level.Info, $"  - # of Styles: &-e;{subtitle.Styles.Count}&-^;");
                Output.WriteLine(Level.Info, $"  - # of Events: &-e;{subtitle.Events.Count}&-^;");
                Output.WriteLine(Level.Info);

                // Write modified subtitles.
                if (!string.IsNullOrWhiteSpace(options.Output)) {

                    string filepath = Path.Combine(options.Output, Path.GetFileName(file)).Replace('\\', '/');

                    Output.WriteLine(Level.Info, $"  - Writing modified subtitles: &-3;{filepath}&-^;");
                    if (!options.Overwrite && File.Exists(filepath)) {
                        Output.WriteLine(Level.Warn, "    - Skipping writing file. File exists! Use the &-a;--overwrite&-^; switch to allow.");
                    } else {
                        Directory.CreateDirectory(options.Output);
                        writer.WriteTo(filepath, subtitle);
                    }

                } else {
                    Output.WriteLine(Level.Warn, "No output template specified (&-a;-o&-^;). Skipping writing subtitles...");
                }



            }

            return 0;
        }

        private bool IsFiltered(object obj, out bool hadError) {
            hadError = false;

            foreach (Filter filter in filters) {

                PropertyInfo info = properties[filter.PropertyName];

                object propertyValue = info.GetValue(obj);

                bool isFiltered = filter.IsFiltered(propertyValue, out string errorMessage);

                if (errorMessage != null) {
                    hadError = true;
                    return false;
                } else if (isFiltered)
                    return true;

            }

            return false;
        }

        private bool TryApplySetters(object obj) {
            foreach (Setter setter in setters) {

                PropertyInfo info = properties[setter.PropertyName];

                if (!SsaReader.TrySetProperty(obj, info, setter.Value)) {
                    Output.WriteLine(Level.Severe, $"Setter: Invalid property value &-e;{setter.Value}&-^; for &-e;{setter.PropertyName}&-^;");
                    return false;
                }

            }
            return true;
        }

        private int RunInfo(InfoOptions options) {

            Output.WriteLine(Level.Severe, "Info verb isn't implement yet!");

            return -1;
        }

        private bool CheckReaderErrors() {
            if (!reader.HasErrors)
                return false;

            foreach (SsaError error in reader)
                Output.WriteLine(Level.Error, $"  - {string.Format(error.Message, error.Args.Select(x => $"&-e;{x}&-^;").ToArray())}");

            return true;
        }

        private static int Main(string[] args) {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                args = new string[] { "edit", "-i", @"input-subs.ass" };
#endif

            Program program = new Program();

            return OnReturnCode(Parser.Default.ParseArguments<InfoOptions, EditOptions>(args)
                .MapResult(
                    (InfoOptions options) => { int? code = program.Init(options); return code ?? program.RunInfo(options); },
                    (EditOptions options) => { int? code = program.Init(options); return code ?? program.RunEdit(options); },
                    errors => -1
                ));
        }

        private static int OnReturnCode(int returnCode) {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                Console.ReadKey();
#endif
            return returnCode;
        }

    }

}