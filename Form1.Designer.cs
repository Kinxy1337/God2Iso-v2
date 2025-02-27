namespace God2Iso
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.textOutput = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.buttonGo = new System.Windows.Forms.Button();
            this.listPackages = new System.Windows.Forms.ListBox();
            this.labelOutput = new System.Windows.Forms.Label();
            this.labelDirectories = new System.Windows.Forms.Label();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.buttonClear = new System.Windows.Forms.Button();
            this.progressTotal = new System.Windows.Forms.ProgressBar();
            this.progressIso = new System.Windows.Forms.ProgressBar();
            this.labelIsoProg = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFix = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonScanFolder = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textOutput
            // 
            this.textOutput.ForeColor = System.Drawing.Color.Crimson;
            this.textOutput.Location = new System.Drawing.Point(12, 131);
            this.textOutput.Name = "textOutput";
            this.textOutput.Size = new System.Drawing.Size(353, 20);
            this.textOutput.TabIndex = 1;
            this.textOutput.Text = "D:\\";
            // 
            // buttonAdd
            // 
            this.buttonAdd.ForeColor = System.Drawing.Color.Crimson;
            this.buttonAdd.Location = new System.Drawing.Point(371, 30);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "Add File...";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.ForeColor = System.Drawing.Color.Crimson;
            this.buttonBrowse.Location = new System.Drawing.Point(371, 129);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 3;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.UseVisualStyleBackColor = true;
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // buttonGo
            // 
            this.buttonGo.ForeColor = System.Drawing.Color.Crimson;
            this.buttonGo.Location = new System.Drawing.Point(147, 212);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(164, 23);
            this.buttonGo.TabIndex = 4;
            this.buttonGo.Text = "Convert to ISO";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // listPackages
            // 
            this.listPackages.BackColor = System.Drawing.Color.Gray;
            this.listPackages.ForeColor = System.Drawing.Color.Crimson;
            this.listPackages.FormattingEnabled = true;
            this.listPackages.Location = new System.Drawing.Point(12, 30);
            this.listPackages.Name = "listPackages";
            this.listPackages.Size = new System.Drawing.Size(353, 82);
            this.listPackages.TabIndex = 5;
            this.listPackages.SelectedIndexChanged += new System.EventHandler(this.listPackages_SelectedIndexChanged);
            // 
            // labelOutput
            // 
            this.labelOutput.AutoSize = true;
            this.labelOutput.ForeColor = System.Drawing.Color.Crimson;
            this.labelOutput.Location = new System.Drawing.Point(9, 115);
            this.labelOutput.Name = "labelOutput";
            this.labelOutput.Size = new System.Drawing.Size(85, 13);
            this.labelOutput.TabIndex = 6;
            this.labelOutput.Text = "Output directory:";
            // 
            // labelDirectories
            // 
            this.labelDirectories.AutoSize = true;
            this.labelDirectories.ForeColor = System.Drawing.Color.Crimson;
            this.labelDirectories.Location = new System.Drawing.Point(9, 14);
            this.labelDirectories.Name = "labelDirectories";
            this.labelDirectories.Size = new System.Drawing.Size(80, 13);
            this.labelDirectories.TabIndex = 7;
            this.labelDirectories.Text = "God packages:";
            // 
            // buttonRemove
            // 
            this.buttonRemove.Enabled = false;
            this.buttonRemove.ForeColor = System.Drawing.Color.Crimson;
            this.buttonRemove.Location = new System.Drawing.Point(371, 60);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(75, 23);
            this.buttonRemove.TabIndex = 8;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // buttonClear
            // 
            this.buttonClear.ForeColor = System.Drawing.Color.Crimson;
            this.buttonClear.Location = new System.Drawing.Point(371, 89);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 9;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // progressTotal
            // 
            this.progressTotal.ForeColor = System.Drawing.Color.Crimson;
            this.progressTotal.Location = new System.Drawing.Point(12, 280);
            this.progressTotal.Name = "progressTotal";
            this.progressTotal.Size = new System.Drawing.Size(434, 23);
            this.progressTotal.Step = 1;
            this.progressTotal.TabIndex = 10;
            // 
            // progressIso
            // 
            this.progressIso.ForeColor = System.Drawing.Color.Crimson;
            this.progressIso.Location = new System.Drawing.Point(12, 238);
            this.progressIso.Name = "progressIso";
            this.progressIso.Size = new System.Drawing.Size(434, 23);
            this.progressIso.Step = 1;
            this.progressIso.TabIndex = 11;
            // 
            // labelIsoProg
            // 
            this.labelIsoProg.AutoSize = true;
            this.labelIsoProg.ForeColor = System.Drawing.Color.Crimson;
            this.labelIsoProg.Location = new System.Drawing.Point(9, 222);
            this.labelIsoProg.Name = "labelIsoProg";
            this.labelIsoProg.Size = new System.Drawing.Size(67, 13);
            this.labelIsoProg.TabIndex = 12;
            this.labelIsoProg.Text = "Iso progress:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Crimson;
            this.label1.Location = new System.Drawing.Point(9, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Total progress:";
            // 
            // cbFix
            // 
            this.cbFix.AutoSize = true;
            this.cbFix.ForeColor = System.Drawing.Color.Crimson;
            this.cbFix.Location = new System.Drawing.Point(12, 157);
            this.cbFix.Name = "cbFix";
            this.cbFix.Size = new System.Drawing.Size(195, 17);
            this.cbFix.TabIndex = 15;
            this.cbFix.Text = "Fix \"CreateIsoGood\" broken header";
            this.cbFix.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.Gray;
            this.textBox1.ForeColor = System.Drawing.Color.Crimson;
            this.textBox1.Location = new System.Drawing.Point(11, 309);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(435, 217);
            this.textBox1.TabIndex = 17;
            // 
            // buttonScanFolder
            // 
            this.buttonScanFolder.ForeColor = System.Drawing.Color.Crimson;
            this.buttonScanFolder.Location = new System.Drawing.Point(147, 180);
            this.buttonScanFolder.Name = "buttonScanFolder";
            this.buttonScanFolder.Size = new System.Drawing.Size(164, 23);
            this.buttonScanFolder.TabIndex = 17;
            this.buttonScanFolder.Text = "Scan Games Folder";
            this.buttonScanFolder.UseVisualStyleBackColor = true;
            this.buttonScanFolder.Click += new System.EventHandler(this.buttonScanFolder_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowText;
            this.ClientSize = new System.Drawing.Size(458, 538);
            this.Controls.Add(this.buttonScanFolder);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.cbFix);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelIsoProg);
            this.Controls.Add(this.progressIso);
            this.Controls.Add(this.progressTotal);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.labelDirectories);
            this.Controls.Add(this.labelOutput);
            this.Controls.Add(this.listPackages);
            this.Controls.Add(this.buttonGo);
            this.Controls.Add(this.buttonBrowse);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textOutput);
            this.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "God2Iso vX.X.X";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textOutput;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.ListBox listPackages;
        private System.Windows.Forms.Label labelOutput;
        private System.Windows.Forms.Label labelDirectories;
        private System.Windows.Forms.Button buttonRemove;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.ProgressBar progressTotal;
        private System.Windows.Forms.ProgressBar progressIso;
        private System.Windows.Forms.Label labelIsoProg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbFix;
        private System.Windows.Forms.TextBox textBox1;
    }
}

