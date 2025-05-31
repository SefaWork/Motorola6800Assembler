using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public partial class Assembler {
        private List<byte[]> SecondPass(string[] lines) {

            this.lineIndex = -1;
            this.pc = 0;

            List<byte[]> bytes = [];

            foreach(AssemblerLineData data in this.compiledLines) {
                this.lineIndex++;
                if (data.instruction == null) {
                    bytes.Add([]);
                    continue;
                };

                data.address = this.pc;

                try {
                    bytes.Add(data.instruction.GetData(this, data));
                    data.instruction.IncrementPC(this, data);
                } catch(Exception e) {
                    throw new Exception($"Line {this.lineIndex}: {e.Message}", e);
                }
            }

            this.lineIndex = -1;
            this.pc = 0;
            this.compiledLines.Clear();
            this.labels.Clear();
            this.constants.Clear();
            this.variables.Clear();
            this.endReached = false;

            return bytes;
        }
    }
}
