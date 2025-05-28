using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    public partial class Instruction {

        static Instruction() {
            RegisterInstruction(new Instruction("ldaa")
                .AddOpcode(AddressingModesEnum.Immediate, 0x86)
                .AddOpcode(AddressingModesEnum.Direct, 0x96)
                .AddOpcode(AddressingModesEnum.Indexed, 0xA6)
                .AddOpcode(AddressingModesEnum.Extended, 0xB6)
            );
        }

    }
}
