namespace Maxstupo.SsaUtil {

    public enum Operator {
        Equals,
        LessThan,
        GreaterThan
    }

    public sealed class Filter {

        public string PropertyName { get; }

        public Operator Operator { get; }

        public string[] Values { get; }

        public Filter(string propertyName, Operator oper, string[] values) {
            this.PropertyName = propertyName;
            this.Operator = oper;
            this.Values = values;
        }

    }

}