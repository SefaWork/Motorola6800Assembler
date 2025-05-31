using System.Text;
using System.Windows.Forms;

namespace MotorolaAssembler
{
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {}

        /// <summary>
        /// When the assembly code text box is updated, we should disable saving object code.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void textBox1_TextChanged(object sender, EventArgs e) {
            this.saveObjectCodeButton.Enabled = false;
        }

        
        /// <summary>
        /// Handles interaction with clear button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearButton_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all?", "Confirm Clear", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                assemblyCode.Text = "";
                machineCode.Text = "";
                fullObjectCode.Text = "";
            }
        }

        /// <summary>
        /// Handles interaction with save button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, EventArgs e) {
            // Create a save file dialog.
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

                // Write our assembly code in the file using UTF8 encoding.
                fs.Write(Encoding.UTF8.GetBytes(assemblyCode.Text));
                fs.Close();
            }
        }

        /// <summary>
        /// Handles interaction with load button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadButton_Click_1(object sender, EventArgs e) {
            // Create open file dialog.
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

                // Read our text using UTF8 encoding.
                string text = new UTF8Encoding(true).GetString(info);
                assemblyCode.Text = text;
                fs.Close();
            }
        }

        /// <summary>
        /// Converts a list of byte[] into string for display, with each value seperated by linebreak.
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        private static string LineByLineTranslation(List<byte[]> bl) {
            string constructed = "";

            foreach (byte[] sequence in bl) {
                constructed += BitConverter.ToString(sequence) + System.Environment.NewLine;
            }

            return constructed.ToUpper().Replace('-', ' ');
        }

        /// <summary>
        /// Converts a list of byte[] into string for display, without any linebreaks.
        /// </summary>
        /// <param name="bl"></param>
        /// <returns></returns>
        private static string FullTranslation(List<byte[]> bl) {
            List<byte> allBytes = [];
            foreach (byte[] ba in bl) {
                allBytes.AddRange(ba);
            }

            return BitConverter.ToString(allBytes.ToArray()).ToUpper().Replace('-', ' ');
        }

        /// <summary>
        /// Handles interaction with assemble button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assembleButton_Click(object sender, EventArgs e) {
            // Create a new instance of assembler.
            Assembler assembler = new();

            try {
                // Translate our assembly code and display results.
                List<byte[]> machine = assembler.AssembleText(assemblyCode.Text);
                machineCode.Text = LineByLineTranslation(machine);
                fullObjectCode.Text = FullTranslation(machine);
                this.saveObjectCodeButton.Enabled = true;
            } catch (Exception exc) {
                // Print errors when they show up.
                machineCode.Text = "";
                this.saveObjectCodeButton.Enabled = false;
                MessageBox.Show(exc.Message, "Assembling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Handles interaction with save object code button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveObjectCodeButton_Click(object sender, EventArgs e) {
            // Create save file dialog.
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

                // This is an unnecessary step but we decided to reassemble the code when saving the object code.
                Assembler assembler = new();
                try {
                    List<byte[]> machine = assembler.AssembleText(assemblyCode.Text);
                    List<byte> allBytes = [];

                    foreach (byte[] ba in machine) {
                        allBytes.AddRange(ba);
                    }

                    // The translated object code is directly written into a .bin file.
                    fs.Write(allBytes.ToArray());
                } catch (Exception exc) {
                    MessageBox.Show(exc.Message, "Assembling Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                fs.Close();
            }
        }
    }
}
