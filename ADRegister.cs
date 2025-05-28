using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {
    public class ADRegister {
        
    }
    
    public interface AssemblerDirective {
        string Name { get; }
        byte[] ProcessDirective(Assembler assembler);
    }
}
