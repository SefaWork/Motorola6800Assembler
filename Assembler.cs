using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    public partial class Assembler {

        public int lineIndex;

        /// <summary>
        /// Current list of line data.
        /// </summary>
        public List<AssemblerLineData> compiledLines;

        /// <summary>
        /// List of constants defined with assembler directives.
        /// </summary>
        public Dictionary<string, int> constants;

        /// <summary>
        /// List of labels defined.
        /// </summary>
        public Dictionary<string, AssemblerLineData> labels;

        /// <summary>
        /// List of line data that use variables such as constants or labels.
        /// </summary>
        public List<AssemblerLineData> variables;

        /// <summary>
        /// Program counter.
        /// </summary>
        public int pc;

        /// <summary>
        /// Set to true when .end is reached.
        /// </summary>
        public bool endReached;

        public Assembler() {
            this.lineIndex = 0;
            this.pc = 0;
            this.labels = [];
            this.constants = [];
            this.variables = [];
            this.compiledLines = [];
            this.endReached = false;
        }

        private string[] TokenizeLine(string line) {
            int commentIndex = line.IndexOf(';');
            if (commentIndex >= 0)
                line = line.Substring(0, commentIndex);

            return line.Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        public List<byte[]> AssembleText(string text) {
            string[] lines = text.ToLower().Split('\n');

            this.FirstPass(lines);
            return this.SecondPass(lines);
        }
    }
}
