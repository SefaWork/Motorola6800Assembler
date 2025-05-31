using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    public class WordDirective : AssemblerDirective {
        public WordDirective(string directive) : base(directive) {}

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            string[] split = valueField.Split(',');

            foreach(string entry in split) {
                int value = this.ParseValue(entry);
                if (value < 0 || value > 65535) throw new Exception("Word value out of range. (0-65535)");
            }

            assembler.pc+=split.Length*2;
        }

        public override byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception("Missing value.");
            string valueField = lineData.valueFields[0];

            string[] split = valueField.Split(',');
            byte[] values = new byte[split.Length * 2];

            for(int i=0; i<split.Length; i++) {
                int value = this.ParseValue(split[i]);
                if (value < 0 || value > 65535) throw new Exception("Word value out of range. (0-65535)");
                values[2*i] = (byte)(value >> 8);
                values[2*i + 1] = (byte)value;
            }

            return values;
        }
    }
}
