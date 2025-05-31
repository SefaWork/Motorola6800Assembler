using System.Text;
using System.Windows.Forms;

namespace MotorolaAssembler
{
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            this.saveObjectCodeButton.Enabled = false;
        }

        private void clearButton_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all?", "Confirm Clear", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                assemblyCode.Text = "";
                machineCode.Text = "";
                fullObjectCode.Text = "";
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Assembly file|*.asm|Text file|*.txt";
            saveFileDialog.Title = "Save machine code";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                if (!fs.CanWrite) {
                    MessageBox.Show("This file is read-only.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fs.Close();
                    return;
                }

                fs.Write(Encoding.UTF8.GetBytes(assemblyCode.Text));
                fs.Close();
            }
        }

        private void loadButton_Click_1(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Code file|*.asm;*.txt";
            openFileDialog.Title = "Load an Assembly file";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)openFileDialog.OpenFile();

                byte[] info = new byte[fs.Length];
                int numBytesToRead = (int)fs.Length;
                if (numBytesToRead > 0) {
                    fs.Read(info, 0, numBytesToRead);
                }

                string text = new UTF8Encoding(true).GetString(info);
                assemblyCode.Text = text;
                fs.Close();
            }
        }
        private static string LineByLineTranslation(List<byte[]> bl) {
            string constructed = "";

            foreach (byte[] sequence in bl) {
                constructed += BitConverter.ToString(sequence) + System.Environment.NewLine;
            }

            return constructed.ToUpper().Replace('-', ' ');
        }

        private static string FullTranslation(List<byte[]> bl) {
            List<byte> allBytes = [];
            foreach (byte[] ba in bl) {
                allBytes.AddRange(ba);
            }

            return BitConverter.ToString(allBytes.ToArray()).ToUpper().Replace('-', ' ');
        }

        private void assembleButton_Click(object sender, EventArgs e) {
            Assembler assembler = new();

            try {
                List<byte[]> machine = assembler.AssembleText(assemblyCode.Text);
                machineCode.Text = LineByLineTranslation(machine);
                fullObjectCode.Text = FullTranslation(machine);
                this.saveObjectCodeButton.Enabled = true;
            } catch (Exception exc) {
                machineCode.Text = "";
                this.saveObjectCodeButton.Enabled = false;
                MessageBox.Show(exc.Message, "Assembling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveObjectCodeButton_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary file|*.bin";
            saveFileDialog.Title = "Save machine code";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                if (!fs.CanWrite) {
                    MessageBox.Show("This file is read-only.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    fs.Close();
                    return;
                }

                Assembler assembler = new();
                try {
                    List<byte[]> machine = assembler.AssembleText(assemblyCode.Text);
                    List<byte> allBytes = [];

                    foreach (byte[] ba in machine) {
                        allBytes.AddRange(ba);
                    }

                    fs.Write(allBytes.ToArray());
                } catch (Exception exc) {
                    MessageBox.Show(exc.Message, "Assembling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                fs.Close();
            }
        }
    }
}
