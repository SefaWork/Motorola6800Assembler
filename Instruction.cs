using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    /// <summary>
    /// Class for definition of assembly instructions.
    /// </summary>
    public class Instruction : IInstruction {
        private string _mnemonic;

        public string mnemonic { get => _mnemonic; }

        // The addressing modes this instruction will allow. 0x00 means it cannot use this addressing mode.
        private byte inherent = 0x00;
        private byte relative = 0x00;
        private byte immediate = 0x00;
        private byte direct = 0x00;
        private byte extended = 0x00;
        private byte indexed = 0x00;

        public Instruction(string mnemonic) {
            this._mnemonic = mnemonic.ToLower();
        }

        /// <summary>
        /// Sets inherent addressing mode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public Instruction SetInherent(byte opcode) {
            this.inherent = opcode;
            return this;
        }

        /// <summary>
        /// Sets relative addressing mode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public Instruction SetRelative(byte opcode) {
            this.relative = opcode;
            return this;
        }

        /// <summary>
        /// Sets immediate addresing mode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public Instruction SetImmediate(byte opcode) {
            this.immediate = opcode;
            return this;
        }

        /// <summary>
        /// Sets direct addressing mode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public Instruction SetDirect(byte opcode) {
            this.direct = opcode;
            return this;
        }

        /// <summary>
        /// Sets extended addressing mode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public Instruction SetExtended(byte opcode) {
            this.extended = opcode;
            return this;
        }

        /// <summary>
        /// Sets indexed addressing mode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public Instruction SetIndexed(byte opcode) {
            this.indexed = opcode;
            return this;
        }

        /// <summary>
        /// Attempts to get value based on label/constant.
        /// </summary>
        /// <param name="valueField"></param>
        /// <param name="assembler"></param>
        /// <param name="lineData"></param>
        /// <param name="style"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool TryGetValue(string valueField, Assembler assembler, AssemblerLineData lineData, System.Globalization.NumberStyles style, out int result) {
            if (Assembler.LABEL_REGEX.IsMatch(valueField)) {
                // This text matches our label regex, so search for a label/constant.
                lineData.variable ??= valueField;

                if (assembler.constants.TryGetValue(lineData.variable, out int value)) {
                    result = value;
                    return true;
                } else if (assembler.labels.TryGetValue(lineData.variable, out AssemblerLineData? found)) {
                    if(this.relative != 0x00) {
                        // -2 because our PC will be 2 higher in execution.
                        result = found.address - lineData.address - 2;
                    } else {
                        result = found.address;
                    }

                    return true;
                } else {
                    assembler.variables.Add(lineData);
                }

            }

            return int.TryParse(valueField, style, null, out result);
        }

        /// <summary>
        /// Attempts to parse given value.
        /// </summary>
        /// <param name="valueField"></param>
        /// <param name="assembler"></param>
        /// <param name="lineData"></param>
        /// <param name="outValue"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private bool TryParse(string valueField, Assembler assembler, AssemblerLineData lineData, out int? outValue) {
            int val;
            if(valueField.StartsWith('$')) {
                // Base 16
                valueField = valueField[1..];
                if(TryGetValue(valueField, assembler, lineData, System.Globalization.NumberStyles.HexNumber, out val)) {
                    outValue = val;
                    return true;
                }
                 
            } else if(valueField.StartsWith('%')) {
                // Base 2
                valueField = valueField[1..];
                if (TryGetValue(valueField, assembler, lineData, System.Globalization.NumberStyles.BinaryNumber, out val)) {
                    outValue = val;
                    return true;
                }
            } else {
                // Base 10
                if (TryGetValue(valueField, assembler, lineData, System.Globalization.NumberStyles.Integer, out val)) {
                    outValue = val;
                    return true;
                }
            }

            if (lineData.variable == null) throw new Exception("Invalid variable.");

            outValue = null;
            return false;
        }

        /// <summary>
        /// Called in both first pass and second pass. Used for incrementing program counter and setting up labels/variables/constants.
        /// </summary>
        /// <param name="assembler"></param>
        /// <param name="lineData"></param>
        /// <exception cref="Exception"></exception>
        public void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            int size = 1;

            if (lineData.valueFields.Length == 0) {
                // No value was given, it could be inherent addressing.
                if (this.inherent == 0x00) throw new Exception("Missing value.");
                size = 1;
            } else {
                string valueField = lineData.valueFields[0];
                size = 2;
                int? value;

                if (valueField.StartsWith("#")) {
                    // # means immediate.
                    if (this.immediate == 0x00) throw new Exception("Immediate operand not allowed.");

                    // LDX and LDS are stinky rulebreakers.
                    if (this._mnemonic == "ldx" || this._mnemonic == "lds") {
                        size = 3;
                    }

                    valueField = valueField[1..];
                } else if(valueField.EndsWith(",x")) {
                    // ,x means indexed.
                    if (this.indexed == 0x00) throw new Exception("Indexed operand not allowed.");
                    valueField = valueField[..^2];
                } else if(this.extended != 0x00) {
                    // extended addressing mode can occupy 3 bytes of space instead of 2.
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
        
        /// <summary>
        /// Gets byte sequence for this instruction. Only called in second pass.
        /// </summary>
        /// <param name="assembler"></param>
        /// <param name="lineData"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public byte[] GetData(Assembler assembler, AssemblerLineData lineData) {

            if (lineData.valueFields.Length == 0) {
                // No value implies inherent addressing.
                return [this.inherent];
            } else {
                string valueField = lineData.valueFields[0];
                bool isImmediate = false;
                bool isIndexed = false;

                if (valueField.StartsWith("#")) {
                    // # means immediate addressing
                    isImmediate = true;
                    valueField = valueField[1..];
                } else if (valueField.EndsWith(",x")) {
                    // ,x means indexed addressing.
                    isIndexed = true;
                    valueField = valueField[..^2];
                }

                // Parse the value again, this time we will use it.
                if(!TryParse(valueField, assembler, lineData, out int? value)) throw new Exception("Invalid label/constant.");
                if (value == null) throw new Exception("why."); // just making C# compiler obey me >:(

                if (isImmediate) {
                    // LDX and LDS are still stinky rulebreakers.
                    if(this._mnemonic == "ldx" || this._mnemonic == "lds") {
                        if (value < 0 || value > 65535) throw new Exception("Value out of range. 0-65535 (FFFF)");
                        // The operand is split into two bytes.
                        return [this.immediate, (byte)(value >> 8), (byte)(value)];
                    } else {
                        if (value < 0 || value > 255) throw new Exception("Value out of range. 0-255 (FF)");
                        return [this.immediate, (byte)(value)];
                    }
                } else if (isIndexed) {
                    if(value < 0 || value > 255) throw new Exception("Value out of range. 0-255 (FF)");
                    return [this.indexed, (byte)(value)];
                } else if (this.relative != 0x00) {
                    // Relative mode takes priority since instructions that use it do not use extended or direct.
                    if (value < -126 || value > 129) throw new Exception("Value out of range. Must be between -126 to 129 for relative addressing.");

                    return [this.relative, value > 0 ? (byte)(value) : (byte)(256 + value)];
                } else if (this.extended != 0x00) {
                    if (value < 0 || value > 65535) throw new Exception("Value out of range. 0-65535 (FFFF)");

                    // If direct mode is available, this instruction has dynamic size (2 or 3 bytes), otherwise it is always 3 bytes.
                    if (value < 256 && this.direct != 0x00) {
                        return [this.direct, (byte)(value)];
                    } else {
                        // The operand is split into two bytes.
                        return [this.extended, (byte)(value >> 8), (byte)(value)];
                    }
                } else if (this.direct != 0x00) {
                    // Last option is direct addressing.
                    if (value < 0 || value > 255) throw new Exception("Value out of range. 0-255 (FF)");
                    return [this.direct, (byte)(value)];
                } else {
                    throw new Exception("This is a problem with the program. A mnemonic has no defined opcodes.");
                }
            }
        }
    }
}
