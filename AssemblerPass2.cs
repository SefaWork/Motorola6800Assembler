using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public partial class Assembler {

        /// <summary>
        /// This is for the second pass of our algorithm. All the lines we compiled in the first pass will now be processed and converted into machine code.
        /// </summary>
        /// <returns>A list containing sequences of bytes, seperated by line.</returns>
        /// <exception cref="Exception"></exception>
        private List<byte[]> SecondPass() {

            // Reset values.
            this.lineIndex = -1;
            this.pc = 0;

            // Initialize list of bytes.
            List<byte[]> bytes = [];

            // Iterate over each compiled line.
            foreach(AssemblerLineData data in this.compiledLines) {
                this.lineIndex++;

                // If it has no instruction, we just add an empty table to display an empty line.
                if (data.instruction == null) {
                    bytes.Add([]);
                    continue;
                };
                
                // Since the size may have changed (because of direct/extended addressing mode), we update the address.
                data.address = this.pc;

                try {
                    // Convert assembly code into machine code and add it to our list.
                    bytes.Add(data.instruction.GetData(this, data));

                    // And then increment PC.
                    data.instruction.IncrementPC(this, data);
                } catch(Exception e) {
                    throw new Exception($"Line {this.lineIndex}: {e.Message}", e);
                }
            }

            // Reset values.
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
