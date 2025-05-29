using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    /// <summary>
    /// Class for definition of Instructions and holding a record of instructions based on their mnemonics.
    /// </summary>
    public partial class Instruction {
        /// <summary>
        /// List of instructions, indexed by their mnemonic in lowercase.
        /// </summary>
        private static readonly Dictionary<string, Instruction> INSTRUCTIONS = new Dictionary<string, Instruction>();

        private string mnemonic;

        // List of opcodes.
        private byte inherent = 0x00;
        private byte immediate = 0x00;
        private byte relative = 0x00;
        private byte direct = 0x00;
        private byte extended = 0x00;
        private byte indexed = 0x00;

        public static Instruction? GetInstruction(string instruction) {
            if (!INSTRUCTIONS.TryGetValue(instruction.ToLower(), out Instruction? found)) {
                return null;
            } else {
                return found;
            }
        }

        private static void RegisterInstruction(Instruction instruction) {
            INSTRUCTIONS.Add(instruction.mnemonic, instruction);
        }

        private Instruction(string mnemonic) {
            this.mnemonic = mnemonic.ToLower();
        }

        public byte[]? Process(LineProcess process) {
            string? valueString = process.valueString;

            if(valueString == null) {
                if(this.inherent == 0x00) {
                    throw new Exception("Missing value.");
                } else {
                    process.opcode = this.inherent;
                    process.value = 0;
                    return [process.opcode];
                }
            } else if(valueString != null) {
                process.size = 2;
                if (valueString.StartsWith("#")) {
                    // Use immediate mode.
                    if (this.immediate == 0x00) {
                        throw new Exception("Immediate addressing mode not supported.");
                    } else {
                        valueString = valueString.Remove(0, 1);
                        process.opcode = this.immediate;
                        ValueParser.ParseValue(valueString, process);
                    }
                } else if(valueString.EndsWith(",x")) {
                    // Use indexed mode.
                    if (this.indexed == 0x00) {
                        throw new Exception("Indexed addressing mode not supported.");
                    } else {
                        valueString = valueString.Remove(valueString.Length - 2, 2);
                        process.opcode = this.indexed;
                        ValueParser.ParseValue(valueString, process);
                    }
                } else if(this.relative != 0x00) {
                    // Use relative mode.
                    process.opcode = this.relative;
                    process.relative = true;
                    ValueParser.ParseValue(valueString, process);
                } else {
                    // Use direct or extended mode.
                    ValueParser.ParseValue(valueString, process);

                    int? value = process.value;
                    if(this.extended != 0x00) {
                        if(this.direct == 0x00) {
                            process.size = 3;
                            process.opcode = this.extended;
                        } else if(this.direct != 0x00) {
                            if(value != null) {
                                if(value > 255) {
                                    process.size = 3;
                                    process.opcode = this.extended;
                                } else {
                                    process.size = 2;
                                    process.opcode = this.direct;
                                }
                            }
                        }
                        if (value != null && (value < 0x0 || value > 0xFF)) throw new Exception("Value is out of range.");
                    } else if(this.direct != 0x00) {
                        process.size = 2;
                        process.opcode = this.direct;
                        if (value != null && (value > 0xF || value < 0x0)) throw new Exception("Value is out of range.");
                    } else {
                        throw new Exception("Engine error. Instruction is missing addressing modes.");
                    }
                }
            }

            if(process.opcode != 0x00 && process.value != null) {
                if(process.relative) {
                    if(process.value > 0) {
                        return [process.opcode, (byte)(256 + process.value - 2)];
                    } else {
                        return [process.opcode, (byte)(process.value)];
                    }
                } else {
                    if (process.value > 255) {
                        return [process.opcode, (byte)(process.value >> 8), (byte)process.value];
                    } else {
                        return [process.opcode, (byte)process.value];
                    }
                }
            }
            return null;
        }
    }

    public class LineProcess {
        public Instruction instruction;

        public int line = 0;
        public int address = 0;
        public int size = 1;
        public byte opcode = 0x00;

        public string? valueString;
        public string? variable;

        public int? value = null;
        public bool relative = false;

        public LineProcess(Instruction instruction, string? valueString) {
            this.instruction = instruction;
            this.valueString = valueString;
        }
    }
}
