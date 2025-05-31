using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    /// <summary>
    /// Class for definition of Instructions.
    /// </summary>
    public class Instruction : IInstruction {
        private string _mnemonic;

        public string mnemonic { get => _mnemonic; }

        private byte inherent = 0x00;
        private byte relative = 0x00;
        private byte immediate = 0x00;
        private byte direct = 0x00;
        private byte extended = 0x00;
        private byte indexed = 0x00;

        public Instruction(string mnemonic) {
            this._mnemonic = mnemonic.ToLower();
        }

        public Instruction SetInherent(byte opcode) {
            this.inherent = opcode;
            return this;
        }
        public Instruction SetRelative(byte opcode) {
            this.relative = opcode;
            return this;
        }
        public Instruction SetImmediate(byte opcode) {
            this.immediate = opcode;
            return this;
        }
        public Instruction SetDirect(byte opcode) {
            this.direct = opcode;
            return this;
        }
        public Instruction SetExtended(byte opcode) {
            this.extended = opcode;
            return this;
        }
        public Instruction SetIndexed(byte opcode) {
            this.indexed = opcode;
            return this;
        }

        private bool TryGetValue(string valueField, Assembler assembler, AssemblerLineData lineData, System.Globalization.NumberStyles style, out int result) {
            if (Assembler.LABEL_REGEX.IsMatch(valueField)) {
                lineData.variable ??= valueField;

                if (assembler.constants.TryGetValue(lineData.variable, out int value)) {
                    result = value;
                    return true;
                } else if (assembler.labels.TryGetValue(lineData.variable, out AssemblerLineData? found)) {
                    if(this.relative != 0x00) {
                        result = found.address - lineData.address;
                    } else {
                        result = found.address;
                    }

                    return true;
                }
            }

            return int.TryParse(valueField, style, null, out result);
        }

        private bool TryParse(string valueField, Assembler assembler, AssemblerLineData lineData, out int? outValue) {
            Debug.WriteLine(valueField);
            int val;
            if(valueField.StartsWith('$')) {
                valueField = valueField[1..];
                if(TryGetValue(valueField, assembler, lineData, System.Globalization.NumberStyles.HexNumber, out val)) {
                    outValue = val;
                    return true;
                }
                 
            } else if(valueField.StartsWith('%')) {
                valueField = valueField[1..];
                if (TryGetValue(valueField, assembler, lineData, System.Globalization.NumberStyles.BinaryNumber, out val)) {
                    outValue = val;
                    return true;
                }
            } else {
                if (TryGetValue(valueField, assembler, lineData, System.Globalization.NumberStyles.Integer, out val)) {
                    outValue = val;
                    return true;
                }
            }

            if (lineData.variable == null) throw new Exception("Invalid variable.");

            outValue = null;
            return false;
        }

        public void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            int size = 1;

            if (lineData.valueFields.Length == 0) {
                if (this.inherent == 0x00) throw new Exception("Missing value.");
                size = 1;
            } else {
                string valueField = lineData.valueFields[0];
                size = 2;
                int? value;

                if (valueField.StartsWith("#")) {
                    if (this.immediate == 0x00) throw new Exception("Immediate operand not allowed.");
                    valueField = valueField[1..];
                } else if(valueField.EndsWith(",x")) {
                    Debug.WriteLine(this.mnemonic);
                    if (this.indexed == 0x00) throw new Exception("Indexed operand not allowed.");
                    valueField = valueField[..^2];
                } else if(this.extended != 0x00) {
                    if (this.direct == 0x00) {
                        size = 3;
                    } else {
                        if (TryParse(valueField, assembler, lineData, out value)) {
                            if (value < 0 || value > 65535) throw new Exception("Value out of range. 0-65535 (FFFF)");
                            if (value > 255) size = 3;
                        }
                    }
                }

                TryParse(valueField, assembler, lineData, out _);
            }

            assembler.pc += size;
        }

        public byte[] GetData(Assembler assembler, AssemblerLineData lineData) {

            if (lineData.valueFields.Length == 0) {
                return [this.inherent];
            } else {
                string valueField = lineData.valueFields[0];
                bool isImmediate = false;
                bool isIndexed = false;

                if (valueField.StartsWith("#")) {
                    isImmediate = true;
                    valueField = valueField[1..];
                } else if (valueField.EndsWith(",x")) {
                    isIndexed = true;
                    valueField = valueField[..^2];
                }

                if(!TryParse(valueField, assembler, lineData, out int? value)) throw new Exception("Invalid label/constant.");
                if (value == null) throw new Exception("why.");

                if (isImmediate) {
                    if(this._mnemonic == "ldx" || this._mnemonic == "lds") {
                        // LDX and LDS are complete rule breakers for immediate addressing. (They allow 16bit value)
                        if (value < 0 || value > 65535) throw new Exception("Value out of range. 0-65535 (FFFF)");
                        return [this.immediate, (byte)(value >> 8), (byte)(value)];
                    } else {
                        if (value < 0 || value > 255) throw new Exception("Value out of range. 0-255 (FF)");
                        return [this.immediate, (byte)(value)];
                    }
                } else if (isIndexed) {
                    if(value < 0 || value > 255) throw new Exception("Value out of range. 0-255 (FF)");
                    return [this.indexed, (byte)(value)];
                } else if (this.relative != 0x00) {
                    if (value < -126 || value > 129) throw new Exception("Value out of range. Must be between -126 to 129 for relative addressing.");

                    return [this.relative, value > 0 ? (byte)(value) : (byte)(254 + value)];
                } else if (this.extended != 0x00) {
                    if (value < 0 || value > 65535) throw new Exception("Value out of range. 0-65535 (FFFF)");

                    if (value < 256 && this.direct != 0x00) {
                        return [this.direct, (byte)(value)];
                    } else {
                        return [this.extended, (byte)(value >> 8), (byte)(value)];
                    }
                } else if (this.direct != 0x00) {
                    if (value < 0 || value > 255) throw new Exception("Value out of range. 0-255 (FF)");
                    return [this.direct, (byte)(value)];
                } else {
                    throw new Exception("This is a problem with the program. A mnemonic has no defined opcodes.");
                }
            }
        }
    }
}
