namespace CompilerProject
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private SplitContainer mainSplitContainer;
        private RichTextBox txtSourceCode;
        private TabControl tabControl1;
        private TabPage tabErrors;
        private ListView lvErrors;
        private TabPage tabOptimized;
        private RichTextBox txtOptimizedCode;
        private Button btnCompile;
        private Button btnClear;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            mainSplitContainer = new SplitContainer();
            txtSourceCode = new RichTextBox();
            tabControl1 = new TabControl();
            tabErrors = new TabPage();
            lvErrors = new ListView();
            tabOptimized = new TabPage();
            txtOptimizedCode = new RichTextBox();
            btnCompile = new Button();
            btnClear = new Button();
            statusStrip1 = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            tabControl1.SuspendLayout();
            tabErrors.SuspendLayout();
            tabOptimized.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.Location = new Point(0, 0);
            mainSplitContainer.Margin = new Padding(3, 4, 3, 4);
            mainSplitContainer.Name = "mainSplitContainer";
            mainSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(txtSourceCode);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(tabControl1);
            mainSplitContainer.Panel2.Controls.Add(btnCompile);
            mainSplitContainer.Panel2.Controls.Add(btnClear);
            mainSplitContainer.Size = new Size(1125, 885);
            mainSplitContainer.SplitterDistance = 535;
            mainSplitContainer.SplitterWidth = 5;
            mainSplitContainer.TabIndex = 0;
            // 
            // txtSourceCode
            // 
            txtSourceCode.Dock = DockStyle.Fill;
            txtSourceCode.Font = new Font("Consolas", 11F);
            txtSourceCode.ForeColor = SystemColors.WindowText;
            txtSourceCode.Location = new Point(0, 0);
            txtSourceCode.Name = "txtSourceCode";
            txtSourceCode.Size = new Size(1125, 535);
            txtSourceCode.TabIndex = 0;
            txtSourceCode.Text = "";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabErrors);
            tabControl1.Controls.Add(tabOptimized);
            tabControl1.Dock = DockStyle.Top;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1125, 295);
            tabControl1.TabIndex = 4;
            // 
            // tabErrors
            // 
            tabErrors.Controls.Add(lvErrors);
            tabErrors.Location = new Point(4, 29);
            tabErrors.Name = "tabErrors";
            tabErrors.Padding = new Padding(3);
            tabErrors.Size = new Size(1117, 262);
            tabErrors.TabIndex = 0;
            tabErrors.Text = "Errores";
            tabErrors.UseVisualStyleBackColor = true;
            // 
            // lvErrors
            // 
            lvErrors.Dock = DockStyle.Fill;
            lvErrors.HideSelection = false;
            lvErrors.Location = new Point(3, 3);
            lvErrors.Name = "lvErrors";
            lvErrors.Size = new Size(1111, 256);
            lvErrors.TabIndex = 0;
            lvErrors.UseCompatibleStateImageBehavior = false;
            lvErrors.DoubleClick += lvErrors_DoubleClick;
            // 
            // tabOptimized
            // 
            tabOptimized.Controls.Add(txtOptimizedCode);
            tabOptimized.Location = new Point(4, 29);
            tabOptimized.Name = "tabOptimized";
            tabOptimized.Padding = new Padding(3);
            tabOptimized.Size = new Size(1117, 262);
            tabOptimized.TabIndex = 1;
            tabOptimized.Text = "Código Optimizado";
            tabOptimized.UseVisualStyleBackColor = true;
            // 
            // txtOptimizedCode
            // 
            txtOptimizedCode.Dock = DockStyle.Fill;
            txtOptimizedCode.Font = new Font("Consolas", 10F);
            txtOptimizedCode.Location = new Point(3, 3);
            txtOptimizedCode.Name = "txtOptimizedCode";
            txtOptimizedCode.Size = new Size(1111, 256);
            txtOptimizedCode.TabIndex = 0;
            txtOptimizedCode.Text = "";
            // 
            // btnCompile
            // 
            btnCompile.Location = new Point(3, 380);
            btnCompile.Name = "btnCompile";
            btnCompile.Size = new Size(150, 40);
            btnCompile.TabIndex = 0;
            btnCompile.Text = "Compilar (F5)";
            btnCompile.UseVisualStyleBackColor = true;
            btnCompile.Click += btnCompile_Click_1;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(160, 380);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(150, 40);
            btnClear.TabIndex = 1;
            btnClear.Text = "Limpiar";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click_1;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { lblStatus });
            statusStrip1.Location = new Point(0, 863);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1125, 22);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(112, 17);
            lblStatus.Text = "Estado del compilador";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 885);
            Controls.Add(mainSplitContainer);
            Controls.Add(statusStrip1);
            Name = "Form1";
            Text = "Compilador Ewe";
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabErrors.ResumeLayout(false);
            tabOptimized.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
