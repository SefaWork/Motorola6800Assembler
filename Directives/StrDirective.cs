using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    /// <summary>
    /// This class implements the .str directive.
    /// </summary>
    public class StrDirective : AssemblerDirective {
        public StrDirective() : base("str") {}

        private byte[] StringToByteArray(string str) {
            return Encoding.ASCII.GetBytes(str);
        }

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception(".str directive expects a string value.");

            string joined = String.Join(String.Empty, lineData.valueFields);
            if (!joined.StartsWith('"') || !joined.EndsWith('"')) throw new Exception(".str directive expects a string value enclosed with quotation marks.");

            joined = joined[1..^1];
            assembler.pc += StringToByteArray(joined).Length;
        }

        public override byte[] GetData(Assembler assembler, AssemblerLineData lineData) {
            if (lineData.valueFields.Length == 0) throw new Exception(".str directive expects a string value.");

            string joined = String.Join(String.Empty, lineData.valueFields);
            if (!joined.StartsWith('"') || !joined.EndsWith('"')) throw new Exception(".str directive expects a string value enclosed with quotation marks.");

            joined = joined[1..^1];
            return StringToByteArray(joined);
        }
    }
}
