namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Represents a property that will be read/written by <see cref="SsaReader"/> or <see cref="SsaWriter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SsaPropertyAttribute : Attribute {

        public string ScriptPropertyName { get; }

        private int order;

        /// <summary>
        /// If <paramref name="ssaPropertyName"/> is null use the name of the property this attribute is on.<br/><br/>
        /// Note: that the <paramref name="order"/> param is auto set to the line number. See <see cref="CallerLineNumberAttribute"/>.
        /// </summary>
        public SsaPropertyAttribute(string ssaPropertyName = null, [CallerLineNumber] int order = 0) {
            this.ScriptPropertyName = ssaPropertyName;
            this.order = order;
        }

        /// <summary>
        /// Returns a dictionary of <see cref="PropertyInfo"/> objects for the specified <paramref name="type"/> (only properties with the <see cref="SsaPropertyAttribute"/> specified). The keys of the dictionary will be the name given to the <see cref="SsaPropertyAttribute"/> or the property name if the attribute name is null.
        /// </summary>
        public static Dictionary<string, PropertyInfo> GetSsaProperties(Type type) {
            Dictionary<string, PropertyInfo> pairs = new Dictionary<string, PropertyInfo>();

            foreach (PropertyInfo x in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
                SsaPropertyAttribute propertyDefintion = x.GetCustomAttribute<SsaPropertyAttribute>();

                if (propertyDefintion == null)
                    continue;

                pairs[propertyDefintion.ScriptPropertyName ?? x.Name] = x;
            }

            return pairs;
        }

        /// <summary>
        /// Returns an ordered list of <see cref="Tuple"/> objects, each containing the name and <see cref="PropertyInfo"/> for the specified <paramref name="type"/> (only properties with the <see cref="SsaPropertyAttribute"/> specified). The names will be the name given to the <see cref="SsaPropertyAttribute"/> or the property name if the attribute name is null.
        /// </summary>
        public static List<Tuple<string, PropertyInfo>> GetSortedSsaProperties(Type type) {
            List<Tuple<string, PropertyInfo>> list = new List<Tuple<string, PropertyInfo>>();

            foreach (PropertyInfo x in type.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(x => x.GetCustomAttribute<SsaPropertyAttribute>()?.order ?? 0)) {
                SsaPropertyAttribute propertyDefintion = x.GetCustomAttribute<SsaPropertyAttribute>();

                if (propertyDefintion == null)
                    continue;

                list.Add(Tuple.Create(propertyDefintion.ScriptPropertyName ?? x.Name, x));
            }

            return list;
        }

    }

}