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

        private void FirstPass(string[] lines) {
            this.pc = 0;
            this.lineIndex = -1;
            this.labels.Clear();
            this.variables.Clear();
            this.constants.Clear();
            this.compiledLines.Clear();
            this.endReached = false;

            foreach(string line in lines) {
                this.lineIndex++;
                AssemblerLineData data = new AssemblerLineData() { lineNum = this.lineIndex, address = this.pc };
                this.compiledLines.Add(data);

                string trimmed = line.Trim();
                if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith(';')) continue;

                string[] tokens = this.TokenizeLine(line);
                if (tokens.Length < 1) continue;

                string instruction;

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

                data.instruction = foundInstruction;
                try {
                    foundInstruction.IncrementPC(this, data);
                } catch (Exception e) {
                    throw new Exception($"Line {this.lineIndex}: {e.Message}", e);
                }
                if (this.endReached) break;
            }
        }
    }
}
