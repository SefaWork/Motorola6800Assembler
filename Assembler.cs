using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    public partial class Assembler {

        private static readonly Regex labelRegex = new Regex(@"^[a-zA-Z]+$");

        private int lineIndex;
        private List<LineProcess> processList;
        private Dictionary<string, int> labels;

        public Assembler() {
            this.lineIndex = 0;
            this.labels = [];
            this.processList = [];
        }

        private string[] TokenizeLine(string line) {
            return line.Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        public List<byte> AssembleText(string text) {
            string[] lines = text.Split('\n');

            this.FirstPass(lines);
            return this.SecondPass(lines);
        }
    }
}
