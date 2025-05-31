using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    /// <summary>
    /// This class implements .rmb directive.
    /// </summary>
    public class RmbDirective : AssemblerDirective {
        public RmbDirective() : base("rmb") {}

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            int value = this.ParseValue(valueField);
            if (value < 1) throw new Exception("Cannot reserve 0 or less bytes.");

            assembler.pc += value;
        }

        public override byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            int value = this.ParseValue(valueField);
            if (value < 1) throw new Exception("Cannot reserve 0 or less bytes.");

            return [.. Enumerable.Repeat((byte)0x00, value)];
        }
    }
}
