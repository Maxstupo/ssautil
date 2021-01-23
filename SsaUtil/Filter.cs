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

        public bool IsFiltered(object propertyValue, out string errorMessage) {
            bool isNumeric = propertyValue.GetType() == typeof(float) || propertyValue.GetType() == typeof(int);

            errorMessage = null;

            bool isFiltered = true;

            float nValue = 0;
            foreach (string textValue in Values) {

                if (isNumeric && !float.TryParse(textValue, out nValue)) {
                    errorMessage = $"Filter: User-defined value &-e;{textValue}&-^; isn't numeric for property &-e;{PropertyName}&-^;";
                    return false;
                }

                switch (Operator) {
                    case Operator.Equals:

                        if (isNumeric) {
                            if ((float) propertyValue == nValue)
                                isFiltered = false;

                        } else if (propertyValue.ToString() == textValue) {
                            isFiltered = false;

                        }

                        break;

                    case Operator.LessThan:

                        if (!isNumeric) {
                            errorMessage = $"Filter: Invalid operator for non-numeric value!";
                            return false;
                        }

                        if ((float) propertyValue < nValue)
                            isFiltered = false;

                        break;

                    case Operator.GreaterThan:
                        if (!isNumeric) {
                            errorMessage = $"Filter: Invalid operator for non-numeric value!";
                            return false;
                        }

                        if ((float) propertyValue > nValue)
                            isFiltered = false;

                        break;
                    default:
                        break;
                }
            }

            return isFiltered;

        }

    }

}