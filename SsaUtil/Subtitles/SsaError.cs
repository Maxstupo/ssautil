namespace Maxstupo.SsaUtil.Subtitles {

    using System;
    using System.Collections.Generic;

    /// <summary>An error that occurred during reading / writing of an SSA subtitle file.</summary>
    public sealed class SsaError : IEquatable<SsaError> {

        /// <summary>The line number the error occurred on. </summary>
        public int LineNumber { get; }

        /// <summary>The error message. Standard string formatting supported <see cref="string.Format(string, object[])"/>.</summary>
        public string Message { get; }

        /// <summary>Object arguments used in the message.</summary>
        public object[] Args { get; }


        public SsaError(string message, int lineNumber, object[] args) {
            this.Message = message;
            this.LineNumber = lineNumber;
            this.Args = args;
        }

        /// <summary>Returns the formatted error message with arguments filled in. See <see cref="string.Format(string, object[])"/></summary>
        public override string ToString() {
            return string.Format(Message, Args);
        }

        public override bool Equals(object obj) {
            return Equals(obj as SsaError);
        }

        public bool Equals(SsaError other) {
            return other != null &&
                   this.LineNumber == other.LineNumber &&
                   this.Message == other.Message &&
                   EqualityComparer<object[]>.Default.Equals(this.Args, other.Args);
        }

        public override int GetHashCode() {
            int hashCode = 317449670;
            hashCode = hashCode * -1521134295 + this.LineNumber.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Message);
            hashCode = hashCode * -1521134295 + EqualityComparer<object[]>.Default.GetHashCode(this.Args);
            return hashCode;
        }

        public static bool operator ==(SsaError left, SsaError right) {
            return EqualityComparer<SsaError>.Default.Equals(left, right);
        }

        public static bool operator !=(SsaError left, SsaError right) {
            return !(left == right);
        }

    }

}