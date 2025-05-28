using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MotorolaAssembler {

    public class Assembler {

        private static readonly Regex labelRegex = new Regex(@"^[a-zA-Z]+$");

        private int lineIndex;
        private int addressIndex;
        private List<byte> machineCode;
        private Dictionary<string, int> labels;

        public Assembler() {
            this.lineIndex = 0;
            this.addressIndex = 0;
            this.labels = [];
            this.machineCode = [];
        }

        private string[] TokenizeLine(string line) {
            return line.Trim().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        }

        private void ProcessLabels(string line) {
            string[] tokens = TokenizeLine(line);
            string firstToken = tokens[0];

            if (!line.StartsWith(' ') && !line.StartsWith('\t')) {

            }
        }

        private void ProcessLine(string line) {
            string[] tokens = TokenizeLine(line);
            string firstToken = tokens[0];

            byte[] lineCode;

            if (!line.StartsWith(' ') && !line.StartsWith('\t')) {
                lineCode = ProcessTokens(1, tokens);
            } else {
                lineCode = ProcessTokens(0, tokens);
            }

            foreach (byte entry in lineCode) {
                machineCode.Add(entry);
            }
            this.addressIndex += lineCode.Length;
        }

        private byte[] ProcessTokens(int startIndex, string[] tokens) {
            string mnemonic = tokens[startIndex];
            string value = tokens[startIndex + 1];

            Instruction? instr = Instruction.GetInstruction(mnemonic) ?? throw new Exception($"Line {lineIndex}: Instruction not found.");

            try {
                byte[] val = instr.ProcessValue(value, this.labels);
                return val;
            } catch (Exception e) {
                throw new Exception($"Line {lineIndex}: {e.Message}");
            }
        }

        public List<byte> AssembleText(string text) {
            string[] lines = text.Split('\n');

            this.labels.Clear();
            this.lineIndex = 0;
            this.addressIndex = 0;
            this.machineCode.Clear();

            foreach (string line in lines) {
                this.ProcessLabels(line);
                this.lineIndex++;
            }

            this.lineIndex = 0;
            this.addressIndex = 0;

            foreach (string line in lines) {
                this.ProcessLine(line);
                this.lineIndex++;
            }

            return this.machineCode;
        }
    }
}
