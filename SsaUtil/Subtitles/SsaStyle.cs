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

    public enum BorderStyle : int {
        OutlineDropShadow = 1,
        OpaqueBox = 3,
    }

    public class SsaStyle : IEquatable<SsaStyle> {

        /// <summary>The name of the Style. Case sensitive. Cannot include commas.</summary>
        [SsaProperty] public string Name { get; set; }

        /// <summary>The fontname as used by Windows. Case-sensitive.</summary>
        [SsaProperty("Fontname")] public string FontName { get; set; }

        [SsaProperty("Fontsize")] public float FontSize { get; set; }

        /// <summary>This is the colour that a subtitle will normally appear in.</summary>
        [SsaProperty] public string PrimaryColour { get; set; }

        /// <summary>
        /// This colour may be used instead of the Primary colour when a subtitle is automatically shifted 
        /// to prevent an onscreen collsion, to distinguish the different subtitles.
        /// </summary>
        [SsaProperty] public string SecondaryColour { get; set; }

        /// <summary>
        /// This colour may be used instead of the Primary or Secondary colour when a subtitle is automatically shifted 
        /// to prevent an onscreen collsion, to distinguish the different subtitles.
        /// </summary>
        [SsaProperty] public string OutlineColour { get; set; }
        [SsaProperty] public string BackColour { get; set; }

        [SsaProperty] public bool Bold { get; set; }
        [SsaProperty] public bool Italic { get; set; }
        [SsaProperty] public bool Underline { get; set; }
        [SsaProperty] public bool StrikeOut { get; set; }

        /// <summary>Modifies the width of the font. [percent]</summary>
        [SsaProperty("ScaleX")] public float ScaleWidth { get; set; }

        /// <summary>Modifies the height of the font. [percent]</summary>
        [SsaProperty("ScaleY")] public float ScaleHeight { get; set; }

        /// <summary>Extra space between characters.</summary>
        [SsaProperty] public float Spacing { get; set; }

        /// <summary>The origin of the rotation is defined by <see cref="Alignment"/>. [degrees]</summary>
        [SsaProperty] public float Angle { get; set; }


        [SsaProperty] public BorderStyle BorderStyle { get; set; } = BorderStyle.OutlineDropShadow;

        /// <summary>If BorderStyle is <see cref="BorderStyle.OutlineDropShadow"/>, then this specifies the width of the outline around the text.</summary>
        [SsaProperty] public float Outline { get; set; }

        /// <summary>
        /// If BorderStyle is <see cref="BorderStyle.OutlineDropShadow"/>,  then this specifies the depth of the drop shadow behind the text.<br/>
        /// Drop shadow is always used in addition to an outline - SSA will force an outline of 1 pixel if no outline width is given.
        /// </summary>
        [SsaProperty] public float Shadow { get; set; }

        /// <summary>This sets how text is "justified" within the Left/Right onscreen margins, and also the vertical placing.</summary>
        [SsaProperty] public StyleAlignment Alignment { get; set; }

        /// <summary>
        /// This defines the Left Margin in pixels. It is the distance from the left-hand edge of the screen. 
        /// The three onscreen margins (MarginL, MarginR, MarginV) define areas in which the subtitle text will be displayed.
        /// </summary>
        [SsaProperty("MarginL")] public int LeftMargin { get; set; }

        /// <summary>
        /// This defines the Right Margin in pixels. It is the distance from the right-hand edge of the screen. 
        /// The three onscreen margins (MarginL, MarginR, MarginV) define areas in which the subtitle text will be displayed.
        /// </summary>
        [SsaProperty("MarginR")] public int RightMargin { get; set; }

        /// <summary>
        /// This defines the vertical Left Margin in pixels.<br/><br/>
        /// For a subtitle, it is the distance from the bottom of the screen.<br/>
        /// For a toptitle, it is the distance from the top of the screen.<br/>
        /// For a midtitle, the value is ignored - the text will be vertically centred.
        /// </summary>
        [SsaProperty("MarginV")] public int VerticalMargin { get; set; }

        /// <summary>
        /// This specifies the font character set or encoding and on multi-lingual Windows installations it provides access
        /// to characters used in multiple than one languages.
        /// </summary>
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
            hashCode = hashCode * -1521134295 + this.BorderStyle.GetHashCode();
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