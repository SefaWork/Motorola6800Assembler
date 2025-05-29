using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public partial class Assembler {
        private List<byte> SecondPass(string[] lines) {
            int pc = 0;

            this.lineIndex = 0;
            List<byte> byteList = new List<byte>();

            foreach(LineProcess process in processList) {
                int diff = pc - process.address;
                if(process.variable != null) {
                    if (!labels.TryGetValue(process.variable, out int val)) throw new Exception($"Line {lineIndex}: Invalid token.");
                    if(process.relative) {
                        process.value = val + diff - pc;
                    } else {
                        process.value = val + diff;
                    }
                }

                process.address = pc;
                byte[]? values = process.instruction.Process(process);
                pc += process.size;

                if(values != null) {
                    foreach(byte sequence in values) {
                        byteList.Add(sequence);
                    }
                }
            }

            return byteList;
        }
    }
}
