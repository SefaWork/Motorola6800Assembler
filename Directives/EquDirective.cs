using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    /// <summary>
    /// This class implements .equ directive.
    /// </summary>
    public class EquDirective : AssemblerDirective {
        public EquDirective() : base("equ") {}

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];
            string? label = lineData.label;
            if (label == null) {
                if (lineData.variable == null) throw new Exception("No label defined for directive.");
                return;
            }
            lineData.label = null;
            lineData.variable = label;
            assembler.labels.Remove(label);

            assembler.constants.Add(label, this.ParseValue(valueField));
        }
    }
}
