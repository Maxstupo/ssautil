namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;
    using System.Reflection;

    /// <summary>
    /// Represents a property that will be read/written by <see cref="SsaReader"/> or <see cref="SsaWriter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SsaPropertyAttribute : Attribute {

        public string ScriptPropertyName { get; }

        /// <summary>
        /// If <paramref name="ssaPropertyName"/> is null use the name of the property this attribute is on.
        /// </summary>
        public SsaPropertyAttribute(string ssaPropertyName = null) {
            this.ScriptPropertyName = ssaPropertyName;
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

    }

}