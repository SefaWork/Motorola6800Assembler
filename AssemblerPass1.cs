using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public partial class Assembler {
        private void FirstPass(string[] lines) {
            int pc = 0;

            this.lineIndex = 0;
            this.processList.Clear();

            foreach(string line in lines) {
                char firstChar = line[0];
                string[] tokens = this.TokenizeLine(line);
                string firstToken = tokens[0];

                string? operationToken = null;
                string? valueToken = null;
                if(firstChar == ';') {
                    continue;
                } else if(firstChar != ' ' && firstChar != '\t') {
                    // register label :)
                    if (!ValueParser.CODEDOMPROVIDER.IsValidIdentifier(firstToken)) throw new Exception($"Line {this.lineIndex}: Invalid token definition.");
                    labels.Add(firstToken, pc);

                    foreach(LineProcess lp in this.processList) {
                        if(lp.variable == firstToken) {
                            if(lp.relative) {
                                lp.value = lp.address - pc;
                            } else {
                                lp.value = pc;
                            }
                        }
                    }

                    operationToken = tokens[1];
                    valueToken = tokens[2];
                } else {
                    operationToken = firstToken;
                    valueToken = tokens[1];
                }

                if (operationToken == null) continue;

                Instruction found = Instruction.GetInstruction(operationToken) ?? throw new Exception($"Line {this.lineIndex}: Unidentified instruction.");

                LineProcess process = new LineProcess(found, valueToken) { address = pc, line = this.lineIndex };
                found.Process(process);

                if(process.variable != null) {
                    if(labels.TryGetValue(process.variable, out int val)) {
                        process.value = val;
                    }
                }

                this.processList.Add(process);

                pc += process.size;
                this.lineIndex++;
            }
        }
    }
}
