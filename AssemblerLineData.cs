using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    public class AssemblerLineData {
        /// <summary>
        /// Name of the label in this line. This is null if line does not contain a label definition.
        /// </summary>
        public string? label = null;

        /// <summary>
        /// The value field provided.
        /// </summary>
        public string[] valueFields = [];

        /// <summary>
        /// The variable this line needs. (Either constant or label)
        /// </summary>
        public string? variable = null;

        /// <summary>
        /// The line number in the assembly code this line originates from.
        /// </summary>
        public int lineNum = 0;

        /// <summary>
        /// The address at which this line begins. (Program counter has not been incremented yet.)
        /// </summary>
        public int address = 0;

        /// <summary>
        /// Either CPU instruction or Assembler directive.
        /// </summary>
        public IInstruction? instruction;
    }

    /// <summary>
    /// Represents an operation that may return an object code representation.
    /// This can include assembler directives and cpu instructions.
    /// </summary>
    public interface IInstruction {
        /// <summary>
        /// Calculates and returns the size of the data in bytes.
        /// </summary>
        /// <returns>The size of the data in bytes. Returns 0 if there is no data.</returns>
        public void IncrementPC(Assembler assembler, AssemblerLineData lineData);

        /// <summary>
        /// Assembles object code and returns it as byte array.
        /// </summary>
        /// <returns>Byte array forming the object code.</returns>
        public byte[] GetData(Assembler assembler, AssemblerLineData lineData);
    }
}
