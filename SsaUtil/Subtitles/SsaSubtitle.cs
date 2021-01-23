namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;


    public enum WrapStyle : int {
        /// <summary>smart wrapping, lines are evenly broken</summary>
        SmartWrappingEven = 0,

        /// <summary>end-of-line word wrapping, only \N breaks</summary>
        EndOfLineWord = 1,

        /// <summary>no word wrapping, \n \N both breaks</summary>
        NoWordWrapping = 2,

        /// <summary>same as <see cref="SmartWrappingEven"/>, but lower line gets wider.</summary>
        SmartWrappingWider = 3
    }

    public class SsaSubtitle<S, E> : IEquatable<SsaSubtitle<S, E>> where S : SsaStyle where E : SsaEvent {

        [SsaProperty] public string ScaledBorderAndShadow { get; set; }

        /// <summary>This is a description of the script.</summary>
        [SsaProperty] public string Title { get; set; }


        /// <summary>The original author(s) of the script.</summary>
        [SsaProperty("Original Script")] public string OriginalScript { get; set; }

        /// <summary>(optional) The original translator of the dialogue.</summary>
        [SsaProperty("Original Translation")] public string OriginalTranslation { get; set; }

        /// <summary>
        /// (optional) The original script editor(s), typically whoever took the raw translation
        /// and turned it into idiomatic english and reworded for readability.
        /// </summary>
        [SsaProperty("Original Editing")] public string OriginalEditing { get; set; }

        /// <summary>(optional) Whoever timed the original script.</summary>
        [SsaProperty("Original Timing")] public string OriginalTiming { get; set; }

        /// <summary>(optional) Description of where in the video the script should begin playback.</summary>
        [SsaProperty("Synch Point")] public string SynchPoint { get; set; }

        /// <summary>(optional) Names of any other subtitling groups who edited the original script.</summary>
        [SsaProperty("Script Updated By")] public string ScriptUpdatedBy { get; set; }

        /// <summary>The details of any updates to the original script - made by other subtitling groups.</summary>
        [SsaProperty("Update Details")] public string UpdateDetails { get; set; }

        /// <summary>
        /// This is the SSA script format version eg. "V4.00". 
        /// It is used by SSA to give a warning if you are using a version of SSA older than the version that created the script.
        /// </summary>
        [SsaProperty] public string ScriptType { get; set; }

        // TODO: Add Collisions ssa property

        /// <summary>
        /// This is the width of the screen used by the script's author(s) when playing the script. 
        /// SSA will automatically select the nearest enabled, setting if you are using Directdraw playback.
        /// </summary>
        [SsaProperty] public int PlayResX { get; set; }

        /// <summary>
        /// This is the height of the screen used by the script's author(s) when playing the script. 
        /// SSA v4 will automatically select the nearest enabled setting, if you are using Directdraw playback.
        /// </summary>
        [SsaProperty] public string PlayResY { get; set; }

        /// <summary>
        /// This is the Timer Speed for the script, as a percentage.
        /// eg. "100.0000" is exactly 100%. It has four digits following the decimal point.
        /// </summary>
        [SsaProperty] public string Timer { get; set; }

        /// <summary>Defines the default wrapping style.</summary>
        [SsaProperty] public WrapStyle WrapStyle { get; set; } = WrapStyle.SmartWrappingEven;

        [SsaProperty("YCbCr Matrix")] public string YCbCrMatrix { get; set; }

        public Dictionary<string, S> Styles { get; } = new Dictionary<string, S>();

        public List<E> Events { get; } = new List<E>();

        public override bool Equals(object obj) {
            return Equals(obj as SsaSubtitle<S, E>);
        }

        public bool Equals(SsaSubtitle<S, E> other) {
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
                   EqualityComparer<Dictionary<string, S>>.Default.Equals(this.Styles, other.Styles) &&
                   EqualityComparer<List<E>>.Default.Equals(this.Events, other.Events);
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
            hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<string, S>>.Default.GetHashCode(this.Styles);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<E>>.Default.GetHashCode(this.Events);
            return hashCode;
        }

        public static bool operator ==(SsaSubtitle<S, E> left, SsaSubtitle<S, E> right) {
            return EqualityComparer<SsaSubtitle<S, E>>.Default.Equals(left, right);
        }

        public static bool operator !=(SsaSubtitle<S, E> left, SsaSubtitle<S, E> right) {
            return !(left == right);
        }
    }

}