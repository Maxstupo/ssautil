namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;

    public class SsaEvent : IEquatable<SsaEvent> {

        public bool IsComment { get; set; }

        [SsaProperty] public string Layer { get; set; }

        [SsaProperty] public string Start { get; set; }
        [SsaProperty] public string End { get; set; }

        [SsaProperty] public string Style { get; set; }

        [SsaProperty] public string Name { get; set; }

        [SsaProperty("MarginL")] public int LeftMargin { get; set; }
        [SsaProperty("MarginR")] public int RightMargin { get; set; }
        [SsaProperty("MarginV")] public int VerticalMargin { get; set; }

        [SsaProperty] public string Effect { get; set; }

        [SsaProperty] public string Text { get; set; }

        public override bool Equals(object obj) {
            return Equals(obj as SsaEvent);
        }

        public bool Equals(SsaEvent other) {
            return other != null &&
                   this.IsComment == other.IsComment &&
                   this.Layer == other.Layer &&
                   this.Start == other.Start &&
                   this.End == other.End &&
                   this.Style == other.Style &&
                   this.Name == other.Name &&
                   this.LeftMargin == other.LeftMargin &&
                   this.RightMargin == other.RightMargin &&
                   this.VerticalMargin == other.VerticalMargin &&
                   this.Effect == other.Effect &&
                   this.Text == other.Text;
        }

        public override int GetHashCode() {
            int hashCode = -1420741783;
            hashCode = hashCode * -1521134295 + this.IsComment.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Layer);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Start);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.End);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Style);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Name);
            hashCode = hashCode * -1521134295 + this.LeftMargin.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightMargin.GetHashCode();
            hashCode = hashCode * -1521134295 + this.VerticalMargin.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Effect);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Text);
            return hashCode;
        }

        public static bool operator ==(SsaEvent left, SsaEvent right) {
            return EqualityComparer<SsaEvent>.Default.Equals(left, right);
        }

        public static bool operator !=(SsaEvent left, SsaEvent right) {
            return !(left == right);
        }

    }

}