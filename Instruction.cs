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
        private Dictionary<AddressingModesEnum, byte> opcodes;

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
            this.opcodes = [];
        }

        private Instruction AddOpcode(AddressingModesEnum addressingMode, byte opcode) {
            this.opcodes.Add(addressingMode, opcode);
            return this;
        }

        public AddressingModesEnum? GetAddressingMode(string? value) {
            AddressingModesEnum? mode = null;
            if(value == null) {
                mode = AddressingModesEnum.Inherent;
            } else {
                string lowercase = value.ToLower();
                if (lowercase.StartsWith("#"))
                    mode = AddressingModesEnum.Immediate;
                else if (lowercase.EndsWith(",x"))
                    mode = AddressingModesEnum.Indexed;
                else if (this.opcodes.ContainsKey(AddressingModesEnum.Relative))
                    return AddressingModesEnum.Relative;
                else {
                    if (this.opcodes.ContainsKey(AddressingModesEnum.Extended)) {
                        if (this.opcodes.ContainsKey(AddressingModesEnum.Direct)) {
                            // check size of input (uhhhhh wtf?)
                        } else {
                            return AddressingModesEnum.Extended;
                        }
                    } else if(this.opcodes.ContainsKey(AddressingModesEnum.Direct)) {
                        return AddressingModesEnum.Direct;
                    }
                }
            }

            if(mode == null)
                return null;
            else if (this.opcodes.ContainsKey((AddressingModesEnum)mode))
                return mode;

            return null;
        }

        public byte[] ProcessValue(string? value, Dictionary<string, int>? labels) {
            if (value == null) {
                // Use inherent mode.
                if(!this.opcodes.TryGetValue(AddressingModesEnum.Inherent, out byte opcode)) {
                    throw new Exception("Operation is missing value.");
                }
                return [opcode];
            } else {
                string lowercase = value.ToLower();
                if (lowercase.StartsWith("#")) {
                    // Use immediate mode.

                    if (!this.opcodes.TryGetValue(AddressingModesEnum.Immediate, out byte opcode)) {
                        throw new Exception("Operation is missing value.");
                    }

                    lowercase = lowercase.Remove(0, 1);
                    int parsedVal = ValueParser.ParseValue(lowercase, labels);

                    if (parsedVal > 255 || parsedVal < 0) {
                        throw new Exception("Value out of range. (00-FF)");
                    }

                    return [opcode, Convert.ToByte(parsedVal)];
                } else if (lowercase.EndsWith(",x")) {
                    // Use indexed mode.
                } else {
                    // Use relative, direct or extended mode.
                }
            }

            return [];
        }
    }
    
    public enum AddressingModesEnum: byte {
        Inherent = 1,
        Immediate,
        Relative,
        Direct,
        Extended,
        Indexed
    }
}
