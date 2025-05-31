using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    /// <summary>
    /// This class is for assembler directives, and it implements IInstruction interface just like Instructions.
    /// </summary>
    public class AssemblerDirective : IInstruction {

        private string _directive;
        public string directive { get => _directive; }

        public AssemblerDirective(string directive) {
            this._directive = directive;
        }

        /// <summary>
        /// Parses a given value. Unlike instructions, assembler directives cannot use symbols like labels or constants.
        /// </summary>
        /// <param name="valueField"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// Gets byte sequence and is intended to be overridden by child classes.
        /// </summary>
        /// <param name="assembler"></param>
        /// <param name="lineData"></param>
        /// <returns></returns>
        public virtual byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            return [];
        }

        /// <summary>
        /// Increments the PC counter. (or not). Intended to be overriden by child classes.
        /// </summary>
        /// <param name="assembler"></param>
        /// <param name="lineData"></param>
        public virtual void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            return;
        }
    }
}
