﻿namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    public class SsaWriter {

        private string ProductVersion => System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();

        public void WriteTo(string filepath, SsaSubtitle subtitle) {
            WriteTo(filepath, Encoding.UTF8, subtitle);
        }

        public void WriteTo(string filepath, Encoding encoding, SsaSubtitle subtitle) {

            Dictionary<string, PropertyInfo> subtitleProperties = SsaPropertyAttribute.GetSsaProperties(subtitle.GetType());
            Dictionary<string, PropertyInfo> styleProperties = SsaPropertyAttribute.GetSsaProperties(typeof(SsaStyle));
            Dictionary<string, PropertyInfo> eventProperties = SsaPropertyAttribute.GetSsaProperties(typeof(SsaEvent));

            using (StreamWriter sw = new StreamWriter(filepath, false, encoding)) {

                sw.WriteLine("[Script Info]");
                sw.WriteLine($"; Script generated by SsaUtil v{ProductVersion}");
                sw.WriteLine("; https://github.com/Maxstupo/ssautil");

                foreach (KeyValuePair<string, PropertyInfo> pair in subtitleProperties)
                    sw.WriteLine($"{pair.Key}: {pair.Value.GetValue(subtitle)}");


                sw.WriteLine();
                sw.WriteLine("[V4+ Styles]");
                sw.WriteLine($"Format: {string.Join(", ", styleProperties.Keys)}");

                foreach (SsaStyle style in subtitle.Styles.Values) {
                    string styleValue = string.Join(",", styleProperties.Values.Select(x => {
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
                sw.WriteLine($"Format: {string.Join(", ", eventProperties.Keys)}");

                foreach (SsaEvent evt in subtitle.Events)
                    sw.WriteLine($"{(evt.IsComment ? "Comment" : "Dialogue")}: {string.Join(",", eventProperties.Values.Select(x => x.GetValue(evt)))}");
          
            }

        }

    }

}