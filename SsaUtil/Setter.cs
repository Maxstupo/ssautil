namespace Maxstupo.SsaUtil {
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public sealed class Setter {

        public string PropertyName { get; }
        public string Value { get; }

        public Setter(string propertyName, string value) {
            this.PropertyName = propertyName;
            this.Value = value;
        }

    }

}