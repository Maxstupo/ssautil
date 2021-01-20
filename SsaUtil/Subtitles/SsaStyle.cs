namespace Maxstupo.SsaUtil.Subtitles {
   
    using System;
    using System.Collections.Generic;

    public enum StyleEncoding : int {
        ANSI = 0,
        Default = 1,
        Symbol = 2,
        Mac = 77,
        Shift_JIS = 128,
        Hangeul = 129,
        Johab = 130,
        GB2312 = 134,
        ChineseBig5 = 136,
        Greek = 161,
        Turkish = 162,
        Vietnamese = 163,
        Hebrew = 177,
        Arabic = 178,
        Baltic = 186,
        Russian = 204,
        Thai = 222,
        EastEuropean = 238,
        OEM = 255
    }

    public enum StyleAlignment : int {
        SouthWest = 1,
        South = 2,
        SouthEast = 3,
        West = 4,
        Center = 5,
        East = 6,
        NorthWest = 7,
        North = 8,
        NorthEast = 9
    }

    public class SsaStyle : IEquatable<SsaStyle> {

        [SsaProperty] public string Name { get; set; }

        [SsaProperty("Fontname")] public string FontName { get; set; }

        [SsaProperty("Fontsize")] public float FontSize { get; set; }

        [SsaProperty] public string PrimaryColour { get; set; }
        [SsaProperty] public string SecondaryColour { get; set; }
        [SsaProperty] public string OutlineColour { get; set; }
        [SsaProperty] public string BackColour { get; set; }

        [SsaProperty] public bool Bold { get; set; }
        [SsaProperty] public bool Italic { get; set; }
        [SsaProperty] public bool Underline { get; set; }
        [SsaProperty] public bool StrikeOut { get; set; }

        [SsaProperty("ScaleX")] public float ScaleWidth { get; set; }

        [SsaProperty("ScaleY")] public float ScaleHeight { get; set; }

        [SsaProperty] public float Spacing { get; set; }
        [SsaProperty] public float Angle { get; set; }

        [SsaProperty] public string BorderStyle { get; set; }
        [SsaProperty] public float Outline { get; set; }
        [SsaProperty] public float Shadow { get; set; }

        [SsaProperty] public StyleAlignment Alignment { get; set; }


        [SsaProperty("MarginL")] public int LeftMargin { get; set; }
        [SsaProperty("MarginR")] public int RightMargin { get; set; }
        [SsaProperty("MarginV")] public int VerticalMargin { get; set; }

        [SsaProperty] public StyleEncoding Encoding { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as SsaStyle);
        }

        public bool Equals(SsaStyle other) {
            return other != null &&
                   this.Name == other.Name &&
                   this.FontName == other.FontName &&
                   this.FontSize == other.FontSize &&
                   this.PrimaryColour == other.PrimaryColour &&
                   this.SecondaryColour == other.SecondaryColour &&
                   this.OutlineColour == other.OutlineColour &&
                   this.BackColour == other.BackColour &&
                   this.Bold == other.Bold &&
                   this.Italic == other.Italic &&
                   this.Underline == other.Underline &&
                   this.StrikeOut == other.StrikeOut &&
                   this.ScaleWidth == other.ScaleWidth &&
                   this.ScaleHeight == other.ScaleHeight &&
                   this.Spacing == other.Spacing &&
                   this.Angle == other.Angle &&
                   this.BorderStyle == other.BorderStyle &&
                   this.Outline == other.Outline &&
                   this.Shadow == other.Shadow &&
                   this.Alignment == other.Alignment &&
                   this.LeftMargin == other.LeftMargin &&
                   this.RightMargin == other.RightMargin &&
                   this.VerticalMargin == other.VerticalMargin &&
                   this.Encoding == other.Encoding;
        }

        public override int GetHashCode() {
            int hashCode = 1734248875;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.FontName);
            hashCode = hashCode * -1521134295 + this.FontSize.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.PrimaryColour);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SecondaryColour);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.OutlineColour);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.BackColour);
            hashCode = hashCode * -1521134295 + this.Bold.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Italic.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Underline.GetHashCode();
            hashCode = hashCode * -1521134295 + this.StrikeOut.GetHashCode();
            hashCode = hashCode * -1521134295 + this.ScaleWidth.GetHashCode();
            hashCode = hashCode * -1521134295 + this.ScaleHeight.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Spacing.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Angle.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.BorderStyle);
            hashCode = hashCode * -1521134295 + this.Outline.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Shadow.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Alignment.GetHashCode();
            hashCode = hashCode * -1521134295 + this.LeftMargin.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightMargin.GetHashCode();
            hashCode = hashCode * -1521134295 + this.VerticalMargin.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Encoding.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(SsaStyle left, SsaStyle right) {
            return EqualityComparer<SsaStyle>.Default.Equals(left, right);
        }

        public static bool operator !=(SsaStyle left, SsaStyle right) {
            return !(left == right);
        }

    }

}