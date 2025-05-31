using MotorolaAssembler.Directives;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public class InstructionRegister {
        private static readonly Dictionary<string, Instruction> INSTRUCTION_REGISTER = [];
        private static readonly Dictionary<string, AssemblerDirective> DIRECTIVE_REGISTER = [];

        private static void Register(Instruction instruction) => INSTRUCTION_REGISTER.Add(instruction.mnemonic, instruction);
        private static void RegisterDirective(AssemblerDirective directive) => DIRECTIVE_REGISTER.Add(directive.directive, directive);

        /// <summary>
        /// Gets instruction by mnemonic from the register.
        /// </summary>
        /// <param name="mnemonic"></param>
        /// <returns></returns>
        public static Instruction? GetInstruction(string mnemonic) {
            if(INSTRUCTION_REGISTER.TryGetValue(mnemonic, out Instruction? found)) {
                return found;
            } else {
                return null;
            }
        }

        /// <summary>
        /// Gets assembler directive by name from the register.
        /// </summary>
        /// <param name="directive"></param>
        /// <returns></returns>
        public static AssemblerDirective? GetDirective(string directive) {
            if (DIRECTIVE_REGISTER.TryGetValue(directive, out AssemblerDirective? found)) {
                return found;
            } else {
                return null;
            }
        }

        // Static constructor to register our instructions and assembler directives.
        static InstructionRegister() {

            // ABA
            Register(new Instruction("aba").SetInherent(0x1B));

            // ADC
            Register(new Instruction("adca")
                .SetImmediate(0x89)
                .SetDirect(0x99)
                .SetIndexed(0xA9)
                .SetExtended(0xB9)
            );

            Register(new Instruction("adcb")
                .SetImmediate(0xC9)
                .SetDirect(0xD9)
                .SetIndexed(0xE9)
                .SetExtended(0xF9)
            );

            // ADD

            Register(new Instruction("adda")
                .SetImmediate(0x8B)
                .SetDirect(0x9B)
                .SetIndexed(0xAB)
                .SetExtended(0xBB)
            );

            Register(new Instruction("addb")
                .SetImmediate(0xCB)
                .SetDirect(0xDB)
                .SetIndexed(0xEB)
                .SetExtended(0xFB)
            );

            // AND

            Register(new Instruction("anda")
                .SetImmediate(0x84)
                .SetDirect(0x94)
                .SetIndexed(0xA4)
                .SetExtended(0xB4)
            );

            Register(new Instruction("andb")
                .SetImmediate(0xC4)
                .SetDirect(0xD4)
                .SetIndexed(0xE4)
                .SetExtended(0xF4)
            );

            // ASL

            Register(new Instruction("asla")
                .SetInherent(0x48)
            );

            Register(new Instruction("aslb")
                .SetInherent(0x58)
            );

            Register(new Instruction("asl")
                .SetIndexed(0x68)
                .SetExtended(0x78)
            );

            // ASR

            Register(new Instruction("asra")
                .SetInherent(0x47)
            );

            Register(new Instruction("asrb")
                .SetInherent(0x57)
            );

            Register(new Instruction("asr")
                .SetIndexed(0x67)
                .SetExtended(0x77)
            );

            // Branches

            Register(new Instruction("bcc").SetRelative(0x24));
            Register(new Instruction("bcs").SetRelative(0x25));
            Register(new Instruction("beq").SetRelative(0x27));
            Register(new Instruction("bge").SetRelative(0x2C));
            Register(new Instruction("bgt").SetRelative(0x2E));
            Register(new Instruction("bhi").SetRelative(0x22));

            Register(new Instruction("ble").SetRelative(0x2F));
            Register(new Instruction("bls").SetRelative(0x23));
            Register(new Instruction("blt").SetRelative(0x2D));
            Register(new Instruction("bmi").SetRelative(0x2B));
            Register(new Instruction("bne").SetRelative(0x26));
            Register(new Instruction("bpl").SetRelative(0x2A));
            Register(new Instruction("bra").SetRelative(0x20));
            Register(new Instruction("bsr").SetRelative(0x8D));
            Register(new Instruction("bvc").SetRelative(0x28));
            Register(new Instruction("bvs").SetRelative(0x29));

            // Bit

            Register(new Instruction("bita")
                .SetImmediate(0x85)
                .SetDirect(0x95)
                .SetIndexed(0xA5)
                .SetExtended(0xB5)
            );

            Register(new Instruction("bitb")
                .SetImmediate(0xC5)
                .SetDirect(0xD5)
                .SetIndexed(0xE5)
                .SetExtended(0xF5)
            );

            // CBA

            Register(new Instruction("cba").SetInherent(0x11));

            // CLC

            Register(new Instruction("clc").SetInherent(0x0C));

            // CLI

            Register(new Instruction("cli").SetInherent(0x0E));

            // CLR

            Register(new Instruction("clra").SetInherent(0x4F));
            Register(new Instruction("clrb").SetInherent(0x5F));
            Register(new Instruction("clr")
                .SetIndexed(0x6F)
                .SetExtended(0x7F)
            );

            // CLV

            Register(new Instruction("clv").SetInherent(0x0A));

            // CMP

            Register(new Instruction("cmpa")
                .SetImmediate(0x81)
                .SetDirect(0x91)
                .SetIndexed(0xA1)
                .SetExtended(0xB1)
            );

            Register(new Instruction("cmpb")
                .SetImmediate(0xC1)
                .SetDirect(0xD1)
                .SetIndexed(0xE1)
                .SetExtended(0xF1)
            );

            // COM

            Register(new Instruction("coma").SetInherent(0x43));
            Register(new Instruction("comb").SetInherent(0x53));
            Register(new Instruction("com")
                .SetIndexed(0x63)
                .SetExtended(0x73)
            );

            // CPX

            Register(new Instruction("cpx")
                .SetImmediate(0x8C)
                .SetDirect(0x9C)
                .SetIndexed(0xAC)
                .SetExtended(0xBC)
            );

            // DAA

            Register(new Instruction("daa").SetInherent(0x19));

            // DEC

            Register(new Instruction("deca").SetInherent(0x4A));
            Register(new Instruction("decb").SetInherent(0x5A));
            Register(new Instruction("dec")
                .SetIndexed(0x6A)
                .SetExtended(0x7A)
            );

            // DES

            Register(new Instruction("des").SetInherent(0x34));

            // DEX

            Register(new Instruction("dex").SetInherent(0x09));

            // EOR

            Register(new Instruction("eora")
                .SetImmediate(0x88)
                .SetDirect(0x98)
                .SetIndexed(0xA8)
                .SetExtended(0xB8)
            );

            Register(new Instruction("eorb")
                .SetImmediate(0xC8)
                .SetDirect(0xD8)
                .SetIndexed(0xE8)
                .SetExtended(0xF8)
            );

            // INC

            Register(new Instruction("inca").SetInherent(0x4C));
            Register(new Instruction("incb").SetInherent(0x5C));
            Register(new Instruction("inc")
                .SetIndexed(0x6C)
                .SetExtended(0x7C)
            );

            // INS

            Register(new Instruction("ins").SetInherent(0x31));

            // INX

            Register(new Instruction("inx").SetInherent(0x08));

            // JMP

            Register(new Instruction("jmp")
                .SetIndexed(0x6E)
                .SetExtended(0x7E)
            );

            // JSR

            Register(new Instruction("jsr")
                .SetIndexed(0xAD)
                .SetExtended(0xBD)
            );

            // LDA

            Register(new Instruction("ldaa")
                .SetImmediate(0x86)
                .SetDirect(0x96)
                .SetIndexed(0xA6)
                .SetExtended(0xB6)
            );

            Register(new Instruction("ldab")
                .SetImmediate(0xC6)
                .SetDirect(0xD6)
                .SetIndexed(0xE6)
                .SetExtended(0xF6)
            );

            // LDS

            Register(new Instruction("lds")
                .SetImmediate(0x8E)
                .SetDirect(0x9E)
                .SetIndexed(0xAE)
                .SetExtended(0xBE)
            );

            // LDX

            Register(new Instruction("ldx")
                .SetImmediate(0xCE)
                .SetDirect(0xDE)
                .SetIndexed(0xEE)
                .SetExtended(0xFE)
            );

            // LSR

            Register(new Instruction("lsra").SetInherent(0x44));
            Register(new Instruction("lsrb").SetInherent(0x54));

            Register(new Instruction("lsr")
                .SetIndexed(0x64)
                .SetExtended(0x74)
            );

            // NEG

            Register(new Instruction("nega").SetInherent(0x40));
            Register(new Instruction("negb").SetInherent(0x50));

            Register(new Instruction("neg")
                .SetIndexed(0x60)
                .SetExtended(0x70)
            );

            // NOP

            Register(new Instruction("nop").SetInherent(0x01));

            // ORA

            Register(new Instruction("oraa")
                .SetImmediate(0x8A)
                .SetDirect(0x9A)
                .SetIndexed(0xAA)
                .SetExtended(0xBA)
            );

            Register(new Instruction("orab")
                .SetImmediate(0xCA)
                .SetDirect(0xDA)
                .SetIndexed(0xEA)
                .SetExtended(0xFA)
            );

            // PSH

            Register(new Instruction("psha").SetInherent(0x36));
            Register(new Instruction("pshb").SetInherent(0x37));

            // PUL

            Register(new Instruction("pula").SetInherent(0x32));
            Register(new Instruction("pulb").SetInherent(0x33));

            // ROL

            Register(new Instruction("rola").SetInherent(0x49));
            Register(new Instruction("rolb").SetInherent(0x59));

            Register(new Instruction("rol")
                .SetIndexed(0x69)
                .SetExtended(0x79)
            );

            // ROR

            Register(new Instruction("rora").SetInherent(0x46));
            Register(new Instruction("rorb").SetInherent(0x56));

            Register(new Instruction("ror")
                .SetIndexed(0x66)
                .SetExtended(0x76)
            );

            // RTI

            Register(new Instruction("rti").SetInherent(0x3B));

            // RTS

            Register(new Instruction("rts").SetInherent(0x39));

            // SBA

            Register(new Instruction("sba").SetInherent(0x10));

            // SBC

            Register(new Instruction("sbca")
                .SetImmediate(0x82)
                .SetDirect(0x92)
                .SetIndexed(0xA2)
                .SetExtended(0xB2)
            );

            Register(new Instruction("sbcb")
                .SetImmediate(0xC2)
                .SetDirect(0xD2)
                .SetIndexed(0xE2)
                .SetExtended(0xF2)
            );

            // SEC

            Register(new Instruction("sec").SetInherent(0x0D));

            // SEI

            Register(new Instruction("sei").SetInherent(0x0F));

            // SEV

            Register(new Instruction("sev").SetInherent(0x0B));

            // STA

            Register(new Instruction("staa")
                .SetDirect(0x97)
                .SetIndexed(0xA7)
                .SetExtended(0xB7)
            );

            Register(new Instruction("stab")
                .SetDirect(0xD7)
                .SetIndexed(0xE7)
                .SetExtended(0xF7)
            );

            // STS

            Register(new Instruction("sts")
                .SetDirect(0x9F)
                .SetIndexed(0xAF)
                .SetExtended(0xBF)
            );

            // STX

            Register(new Instruction("stx")
                .SetDirect(0xDF)
                .SetIndexed(0xEF)
                .SetExtended(0xFF)
            );

            // SUB

            Register(new Instruction("suba")
                .SetImmediate(0x80)
                .SetDirect(0x90)
                .SetIndexed(0xA0)
                .SetExtended(0xB0)
            );

            Register(new Instruction("subb")
                .SetImmediate(0xC0)
                .SetDirect(0xD0)
                .SetIndexed(0xE0)
                .SetExtended(0xF0)
            );

            // SWI

            Register(new Instruction("swi").SetInherent(0x3F));

            // TAB

            Register(new Instruction("tab").SetInherent(0x16));

            // TAP

            Register(new Instruction("tap").SetInherent(0x06));

            // TBA

            Register(new Instruction("tba").SetInherent(0x17));

            // TPA

            Register(new Instruction("tpa").SetInherent(0x07));

            // TST

            Register(new Instruction("tsta").SetInherent(0x4D));
            Register(new Instruction("tstb").SetInherent(0x5D));
            Register(new Instruction("tst")
                .SetIndexed(0x6D)
                .SetExtended(0x7D)
            );

            // TSX
            Register(new Instruction("tsx").SetInherent(0x30));

            // TXS
            Register(new Instruction("txs").SetInherent(0x35));

            // WAI
            Register(new Instruction("wai").SetInherent(0x3E));

            // Assembler directives
            RegisterDirective(new OrgDirective());
            RegisterDirective(new EndDirective());
            RegisterDirective(new EquDirective());
            RegisterDirective(new RmbDirective());
            RegisterDirective(new StrDirective());

            RegisterDirective(new ByteDirective("byte"));
            RegisterDirective(new ByteDirective("stb"));
            RegisterDirective(new WordDirective("word"));
            RegisterDirective(new WordDirective("stw"));

            Debug.WriteLine($"A total of {INSTRUCTION_REGISTER.Count} mnemonics were registered.");
            Debug.WriteLine($"A total of {DIRECTIVE_REGISTER.Count} assembler directives were registered.");
        }
    }
}
