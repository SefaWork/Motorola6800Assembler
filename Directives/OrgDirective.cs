using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    public class OrgDirective : AssemblerDirective {
        public OrgDirective() : base("org") {}

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            int value = this.ParseValue(valueField);
            if (lineData.address > value) throw new Exception("Backward traversal with org is not permitted.");

            assembler.pc = value;
        }

        public override byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            int value = this.ParseValue(valueField) - lineData.address;
            if (value < 0) throw new Exception("Backward traversal with org is not permitted.");

            return [.. Enumerable.Repeat((byte)0x00, value)];
        }
    }
}
