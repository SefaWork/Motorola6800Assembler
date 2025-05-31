using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    /// <summary>
    /// This class implements .byte and .stb directives.
    /// </summary>
    public class ByteDirective : AssemblerDirective {
        public ByteDirective(string directive) : base(directive) {}

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            string[] split = valueField.Split(',');

            foreach(string entry in split) {
                int value = this.ParseValue(entry);
                if (value < 0 || value > 255) throw new Exception("Byte value out of range. (0-255)");
            }

            assembler.pc+=split.Length;
        }

        public override byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            string[] split = valueField.Split(',');
            byte[] values = new byte[split.Length];

            for(int i=0; i<split.Length; i++) {
                int value = this.ParseValue(split[i]);
                if (value < 0 || value > 255) throw new Exception("Byte value out of range. (0-255)");
                values[i] = (byte)value;
            }

            return values;
        }
    }
}
