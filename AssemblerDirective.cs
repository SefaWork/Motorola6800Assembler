using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public class AssemblerDirective : IInstruction {

        private string _directive;
        public string directive { get => _directive; }

        public AssemblerDirective(string directive) {
            this._directive = directive;
        }

        protected int ParseValue(string valueField) {
            if(valueField.StartsWith('$')) {
                valueField = valueField[1..];

                if(int.TryParse(valueField, System.Globalization.NumberStyles.HexNumber, null, out int val)) {
                    return val;
                }
            } else if(valueField.StartsWith('%')) {
                valueField = valueField[1..];

                if (int.TryParse(valueField, System.Globalization.NumberStyles.BinaryNumber, null, out int val)) {
                    return val;
                }
            } else {
                if (int.TryParse(valueField, System.Globalization.NumberStyles.Integer, null, out int val)) {
                    return val;
                }
            }

            throw new Exception("Variables and labels aren't permitted in assembler directives.");
        }

        public virtual byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            return [];
        }

        public virtual void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            return;
        }
    }
}
