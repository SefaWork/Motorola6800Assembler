using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public partial class Assembler {
        public static readonly Regex LABEL_REGEX = new Regex("^[a-z][a-z0-9]*$");

        /// <summary>
        /// This function handles the first pass of the assembler. Iterates over each line, translates as much as it can and defines labels/variables/constants.
        /// </summary>
        /// <param name="lines"></param>
        /// <exception cref="Exception"></exception>
        private void FirstPass(string[] lines) {

            // First, reset every value.
            this.pc = 0;
            this.lineIndex = -1;
            this.labels.Clear();
            this.variables.Clear();
            this.constants.Clear();
            this.compiledLines.Clear();
            this.endReached = false;

            // Iterate over each line.
            foreach(string line in lines) {
                this.lineIndex++;
                AssemblerLineData data = new AssemblerLineData() { lineNum = this.lineIndex, address = this.pc };
                this.compiledLines.Add(data);

                string trimmed = line.Trim();
                // If line is empty, or is full comment, then we can skip it.
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(';')) continue;

                // Tokenize the line.
                string[] tokens = this.TokenizeLine(line);
                if (tokens.Length < 1) continue;

                string instruction;

                // If the line starts with empty space or a tab, then it has no label.
                if(line.StartsWith(' ') || line.StartsWith('\t')) {
                    // First token is the instruction.
                    instruction = tokens[0];
                    data.valueFields = [.. tokens.Skip(1)];
                } else {
                    // First token is label.
                    string label = tokens[0].ToLower();

                    if (!LABEL_REGEX.IsMatch(label)) throw new Exception($"Line {this.lineIndex}: Invalid label name.");
                    data.label = label;

                    labels.Add(label, data);

                    if (tokens.Length < 2) continue;
                    instruction = tokens[1];
                    data.valueFields = [.. tokens.Skip(2)];
                }

                // Now, we need to search for an assembler directive or an instruction.
                IInstruction? foundInstruction = null;

                if (instruction.StartsWith('.')) {
                    // Search assembly directive.
                    foundInstruction = InstructionRegister.GetDirective(instruction[1..]);
                    if (foundInstruction == null) throw new Exception($"Line {this.lineIndex}: Invalid assembler directive.");
                } else {
                    // Search instruction.
                    foundInstruction = InstructionRegister.GetInstruction(instruction);
                    if (foundInstruction == null) throw new Exception($"Line {this.lineIndex}: Invalid instruction.");
                }

                // One was found, so we can continue.
                data.instruction = foundInstruction;
                try {
                    // We tell the instruction handler to increment the PC based on how long the instruction might be.
                    foundInstruction.IncrementPC(this, data);
                } catch (Exception e) {
                    throw new Exception($"Line {this.lineIndex}: {e.Message}", e);
                }

                // This is a flag for when .end assembler directive is reached.
                if (this.endReached) break;
            }
        }
    }
}
