namespace MotorolaAssembler
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            assemblyCode = new TextBox();
            toolStrip1 = new ToolStrip();
            clearButton = new ToolStripButton();
            saveButton = new ToolStripButton();
            loadButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            assembleButton = new ToolStripButton();
            machineCode = new TextBox();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // assemblyCode
            // 
            assemblyCode.AcceptsTab = true;
            assemblyCode.BackColor = Color.White;
            assemblyCode.Font = new Font("Ubuntu Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            assemblyCode.Location = new Point(12, 30);
            assemblyCode.Multiline = true;
            assemblyCode.Name = "assemblyCode";
            assemblyCode.PlaceholderText = "Write your Motorola6800 assembly code in here.";
            assemblyCode.ScrollBars = ScrollBars.Both;
            assemblyCode.Size = new Size(461, 477);
            assemblyCode.TabIndex = 2;
            assemblyCode.WordWrap = false;
            assemblyCode.TextChanged += textBox1_TextChanged;
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { clearButton, saveButton, loadButton, toolStripSeparator1, assembleButton });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(833, 27);
            toolStrip1.TabIndex = 5;
            toolStrip1.Text = "toolStrip1";
            // 
            // clearButton
            // 
            clearButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            clearButton.Image = (Image)resources.GetObject("clearButton.Image");
            clearButton.ImageTransparentColor = Color.Magenta;
            clearButton.Name = "clearButton";
            clearButton.Size = new Size(29, 24);
            clearButton.Text = "toolStripButton1";
            clearButton.ToolTipText = "Clear All";
            clearButton.Click += clearButton_Click;
            // 
            // saveButton
            // 
            saveButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            saveButton.Image = (Image)resources.GetObject("saveButton.Image");
            saveButton.ImageTransparentColor = Color.Magenta;
            saveButton.Name = "saveButton";
            saveButton.Size = new Size(29, 24);
            saveButton.Text = "toolStripButton6";
            saveButton.ToolTipText = "Save Machine Code";
            saveButton.Click += saveButton_Click;
            // 
            // loadButton
            // 
            loadButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            loadButton.Image = (Image)resources.GetObject("loadButton.Image");
            loadButton.ImageTransparentColor = Color.Magenta;
            loadButton.Name = "loadButton";
            loadButton.Size = new Size(29, 24);
            loadButton.Text = "toolStripButton2";
            loadButton.ToolTipText = "Load Assembly Code";
            loadButton.Click += loadButton_Click_1;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 27);
            // 
            // assembleButton
            // 
            assembleButton.Image = (Image)resources.GetObject("assembleButton.Image");
            assembleButton.ImageTransparentColor = Color.Magenta;
            assembleButton.Name = "assembleButton";
            assembleButton.Size = new Size(97, 24);
            assembleButton.Text = "Assemble";
            assembleButton.ToolTipText = "Assemble Code";
            assembleButton.Click += assembleButton_Click;
            // 
            // machineCode
            // 
            machineCode.BackColor = Color.FromArgb(255, 255, 200);
            machineCode.Font = new Font("Ubuntu Mono", 8.999999F, FontStyle.Regular, GraphicsUnit.Point, 0);
            machineCode.Location = new Point(479, 30);
            machineCode.Multiline = true;
            machineCode.Name = "machineCode";
            machineCode.PlaceholderText = "Machine code will show up here after clicking Assemble button.";
            machineCode.ReadOnly = true;
            machineCode.ScrollBars = ScrollBars.Both;
            machineCode.Size = new Size(342, 477);
            machineCode.TabIndex = 6;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(833, 544);
            Controls.Add(machineCode);
            Controls.Add(toolStrip1);
            Controls.Add(assemblyCode);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Motorola6800 Assembler";
            Load += Form1_Load;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox assemblyCode;
        private ToolStrip toolStrip1;
        private ToolStripButton clearButton;
        private ToolStripButton saveButton;
        private ToolStripButton loadButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripButton assembleButton;
        private TextBox machineCode;
    }
}
