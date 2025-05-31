using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler.Directives {
    /// <summary>
    /// This class implements .end directive.
    /// </summary>
    public class EndDirective : AssemblerDirective {
        public EndDirective() : base("end") {}

        public override void IncrementPC(Assembler assembler, AssemblerLineData lineData) {
            assembler.endReached = true;
        }
    }
}
