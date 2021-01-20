namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;

    public class SsaSubtitle : IEquatable<SsaSubtitle> {

        [SsaProperty] public string ScaledBorderAndShadow { get; set; }

        [SsaProperty] public string Title { get; set; }


        [SsaProperty("Original Script")] public string OriginalScript { get; set; }

        [SsaProperty("Original Translation")] public string OriginalTranslation { get; set; }

        [SsaProperty("Original Editing")] public string OriginalEditing { get; set; }

        [SsaProperty("Original Timing")] public string OriginalTiming { get; set; }

        [SsaProperty("Synch Point")] public string SynchPoint { get; set; }

        [SsaProperty("Script Updated By")] public string ScriptUpdatedBy { get; set; }

        [SsaProperty("Update Details")] public string UpdateDetails { get; set; }

        [SsaProperty] public string ScriptType { get; set; }

        [SsaProperty] public int PlayResX { get; set; }
        [SsaProperty] public string PlayResY { get; set; }

        [SsaProperty] public string Timer { get; set; }
        [SsaProperty] public string WrapStyle { get; set; }

        [SsaProperty("YCbCr Matrix")] public string YCbCrMatrix { get; set; }

        public Dictionary<string, SsaStyle> Styles { get; } = new Dictionary<string, SsaStyle>();

        public List<SsaEvent> Events { get; } = new List<SsaEvent>();

        public override bool Equals(object obj) {
            return Equals(obj as SsaSubtitle);
        }

        public bool Equals(SsaSubtitle other) {
            return other != null &&
                   this.ScaledBorderAndShadow == other.ScaledBorderAndShadow &&
                   this.Title == other.Title &&
                   this.OriginalScript == other.OriginalScript &&
                   this.OriginalTranslation == other.OriginalTranslation &&
                   this.OriginalEditing == other.OriginalEditing &&
                   this.OriginalTiming == other.OriginalTiming &&
                   this.SynchPoint == other.SynchPoint &&
                   this.ScriptUpdatedBy == other.ScriptUpdatedBy &&
                   this.UpdateDetails == other.UpdateDetails &&
                   this.ScriptType == other.ScriptType &&
                   this.PlayResX == other.PlayResX &&
                   this.PlayResY == other.PlayResY &&
                   this.Timer == other.Timer &&
                   this.WrapStyle == other.WrapStyle &&
                   this.YCbCrMatrix == other.YCbCrMatrix &&
                   EqualityComparer<Dictionary<string, SsaStyle>>.Default.Equals(this.Styles, other.Styles) &&
                   EqualityComparer<List<SsaEvent>>.Default.Equals(this.Events, other.Events);
        }

        public override int GetHashCode() {
            int hashCode = 705764834;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ScaledBorderAndShadow);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.OriginalScript);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.OriginalTranslation);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.OriginalEditing);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.OriginalTiming);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.SynchPoint);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ScriptUpdatedBy);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.UpdateDetails);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.ScriptType);
            hashCode = hashCode * -1521134295 + this.PlayResX.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.PlayResY);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Timer);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.WrapStyle);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.YCbCrMatrix);
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, SsaStyle>>.Default.GetHashCode(this.Styles);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<SsaEvent>>.Default.GetHashCode(this.Events);
            return hashCode;
        }

        public static bool operator ==(SsaSubtitle left, SsaSubtitle right) {
            return EqualityComparer<SsaSubtitle>.Default.Equals(left, right);
        }

        public static bool operator !=(SsaSubtitle left, SsaSubtitle right) {
            return !(left == right);
        }

    }

}