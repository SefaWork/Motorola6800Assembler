using System.Text;

namespace MotorolaAssembler
{
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {

        }

        private void textBox1_TextChanged(object sender, EventArgs e) {

        }

        private void clearButton_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("Are you sure you want to clear all?", "This cannot cannot be undone.", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes) {
                assemblyCode.Text = "";
            }
        }

        private void saveButton_Click(object sender, EventArgs e) {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary file|*.bin";
            saveFileDialog.Title = "Save machine code";
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "") {
                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();
                byte[] info = new UTF8Encoding(true).GetBytes(assemblyCode.Text);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
        }

        private void loadButton_Click_1(object sender, EventArgs e) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Assembly file|*.asm|Text document|*.txt";
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
        private static string ByteArrayToString(byte[] ba) {
            return BitConverter.ToString(ba).Replace('-', ' ');
        }

        private void assembleButton_Click(object sender, EventArgs e) {
            Assembler assembler = new Assembler();

            try {
                List<byte> machine = assembler.AssembleText(assemblyCode.Text);
                byte[] bytes = machine.ToArray();
                machineCode.Text = ByteArrayToString(bytes);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "An error occured.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
