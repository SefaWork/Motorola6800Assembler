using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    /// <summary>
    /// This class represents all assembler operations. It is partial to make it more readable. See AssemblerPass1.cs and AssemblerPass2.cs
    /// </summary>
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

        /// <summary>
        /// Splits a line of text into individual tokens, excluding comments and whitespace.
        /// </summary>
        /// <remarks>This method removes any text following a semicolon (';'), treating it as a comment. 
        /// Tokens are split based on spaces and tab characters, with leading and trailing whitespace trimmed.</remarks>
        /// <param name="line">The input line of text to tokenize. Cannot be null.</param>
        /// <returns>An array of tokens extracted from the input line. The array will exclude comments and empty entries.</returns>
        private string[] TokenizeLine(string line) {
            int commentIndex = line.IndexOf(';');
            if (commentIndex >= 0)
                line = line.Substring(0, commentIndex);

            return line.Split([' ', '\t'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Processes the input text and assembles it into a list of byte arrays.
        /// </summary>
        /// <remarks>This method performs two passes on the input text: an initial pass to process the
        /// lines and a second pass to generate the final output. Ensure that the input text is properly formatted, as
        /// unexpected formatting may affect the output.</remarks>
        /// <param name="text">The input text to be processed. Each line of the text is converted to lowercase and used in subsequent
        /// processing.</param>
        /// <returns>A list of byte arrays representing the processed text. The exact structure of the byte arrays depends on the
        /// processing performed by the method.</returns>
        public List<byte[]> AssembleText(string text) {
            string[] lines = text.ToLower().Split('\n');

            this.FirstPass(lines);
            return this.SecondPass();
        }
    }
}
