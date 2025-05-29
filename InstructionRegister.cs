using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    public partial class Instruction {

        static Instruction() {
            RegisterInstruction(new Instruction("ldaa") {
                immediate = 0x86,
                direct=0x96,
                indexed=0xA6,
                extended=0xB6
            });
        }

    }
}
