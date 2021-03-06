﻿namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public sealed class SsaWriter : BaseSsaWriter<SsaStyle, SsaEvent> { }

    public class BaseSsaWriter<S, E> where S : SsaStyle where E : SsaEvent {

        public static string ProductVersion {
            get {
                Version version = Assembly.GetEntryAssembly().GetName().Version;
                return $"{version.Major}.{version.Minor}.{version.Build}{(version.Revision > 0 ? $".{version.Revision}" : string.Empty)}";
            }
        }

        public void WriteTo(string filepath, SsaSubtitle<S, E> subtitle) {
            WriteTo(filepath, Encoding.UTF8, subtitle);
        }

        public void WriteTo(string filepath, Encoding encoding, SsaSubtitle<S, E> subtitle) {

            List<Tuple<string, PropertyInfo>> subtitleProperties = SsaPropertyAttribute.GetSortedSsaProperties(subtitle.GetType());
            List<Tuple<string, PropertyInfo>> styleProperties = SsaPropertyAttribute.GetSortedSsaProperties(typeof(S));
            List<Tuple<string, PropertyInfo>> eventProperties = SsaPropertyAttribute.GetSortedSsaProperties(typeof(E));

            using (StreamWriter sw = new StreamWriter(filepath, false, encoding)) {

                sw.WriteLine("[Script Info]");
                sw.WriteLine($"; Script modified by SsaUtil v{ProductVersion}");
                sw.WriteLine("; https://github.com/Maxstupo/ssautil");

                foreach (Tuple<string, PropertyInfo> pair in subtitleProperties)
                    sw.WriteLine($"{pair.Item1}: {pair.Item2.GetValue(subtitle)}");


                sw.WriteLine();
                sw.WriteLine("[V4+ Styles]");
                sw.WriteLine($"Format: {string.Join(", ", styleProperties.Select(x => x.Item1))}");

                foreach (SsaStyle style in subtitle.Styles.Values) {

                    string styleValue = string.Join(",", styleProperties.Select(x => x.Item2).Select(x => {
                        if (x.PropertyType == typeof(bool)) {
                            return (bool) x.GetValue(style) ? "-1" : "0";

                        } else if (x.PropertyType.IsEnum && Type.GetTypeCode(x.PropertyType) == TypeCode.Int32) {
                            return (int) x.GetValue(style);

                        } else {
                            return x.GetValue(style);

                        }
                    }));
                    sw.WriteLine($"Style: {styleValue}");
                }


                sw.WriteLine();
                sw.WriteLine("[Events]");
                sw.WriteLine($"Format: {string.Join(", ", eventProperties.Select(x => x.Item1))}");

                foreach (SsaEvent evt in subtitle.Events)
                    sw.WriteLine($"{(evt.IsComment ? "Comment" : "Dialogue")}: {string.Join(",", eventProperties.Select(x => x.Item2).Select(x => x.GetValue(evt)))}");

            }

        }

    }

}