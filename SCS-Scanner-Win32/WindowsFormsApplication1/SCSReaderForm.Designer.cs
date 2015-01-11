/* 
 * SCS Memory Reader / Analyzer
 * Lecture: Secure Coding, Team 7, Phase 4
 * Chair XXII Software Engineering, Department of Computer Science TU München
 *
 * Author: Elias Tatros
 */

namespace SecCodeSCSExploit
{
    /* UI Code, mostly generated */
    partial class SCSReaderForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.labelBaseAddress = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonOpenFolder = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxDumpSizeBytes = new System.Windows.Forms.TextBox();
            this.radioButtonUpwards = new System.Windows.Forms.RadioButton();
            this.radioButtonDownwards = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxStartAddress = new System.Windows.Forms.TextBox();
            this.textBoxEndAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Scan";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelBaseAddress
            // 
            this.labelBaseAddress.AutoSize = true;
            this.labelBaseAddress.Location = new System.Drawing.Point(132, 18);
            this.labelBaseAddress.Name = "labelBaseAddress";
            this.labelBaseAddress.Size = new System.Drawing.Size(74, 13);
            this.labelBaseAddress.TabIndex = 1;
            this.labelBaseAddress.Text = "base Address:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(13, 44);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(529, 23);
            this.progressBar1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBox1.Location = new System.Drawing.Point(13, 74);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(529, 371);
            this.textBox1.TabIndex = 4;
            // 
            // buttonOpenFolder
            // 
            this.buttonOpenFolder.Location = new System.Drawing.Point(422, 13);
            this.buttonOpenFolder.Name = "buttonOpenFolder";
            this.buttonOpenFolder.Size = new System.Drawing.Size(120, 23);
            this.buttonOpenFolder.TabIndex = 5;
            this.buttonOpenFolder.Text = "Open Dump Folder";
            this.buttonOpenFolder.UseVisualStyleBackColor = true;
            this.buttonOpenFolder.Click += new System.EventHandler(this.buttonOpenFolder_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxDumpSizeBytes);
            this.groupBox1.Location = new System.Drawing.Point(13, 452);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(113, 48);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Area Size (Bytes)";
            // 
            // textBoxDumpSizeBytes
            // 
            this.textBoxDumpSizeBytes.Location = new System.Drawing.Point(6, 19);
            this.textBoxDumpSizeBytes.Name = "textBoxDumpSizeBytes";
            this.textBoxDumpSizeBytes.Size = new System.Drawing.Size(100, 20);
            this.textBoxDumpSizeBytes.TabIndex = 0;
            this.textBoxDumpSizeBytes.Text = "1024";
            // 
            // radioButtonUpwards
            // 
            this.radioButtonUpwards.AutoSize = true;
            this.radioButtonUpwards.Checked = true;
            this.radioButtonUpwards.Location = new System.Drawing.Point(6, 19);
            this.radioButtonUpwards.Name = "radioButtonUpwards";
            this.radioButtonUpwards.Size = new System.Drawing.Size(39, 17);
            this.radioButtonUpwards.TabIndex = 2;
            this.radioButtonUpwards.TabStop = true;
            this.radioButtonUpwards.Text = "Up";
            this.radioButtonUpwards.UseVisualStyleBackColor = true;
            // 
            // radioButtonDownwards
            // 
            this.radioButtonDownwards.AutoSize = true;
            this.radioButtonDownwards.Location = new System.Drawing.Point(52, 19);
            this.radioButtonDownwards.Name = "radioButtonDownwards";
            this.radioButtonDownwards.Size = new System.Drawing.Size(53, 17);
            this.radioButtonDownwards.TabIndex = 3;
            this.radioButtonDownwards.Text = "Down";
            this.radioButtonDownwards.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonUpwards);
            this.groupBox2.Controls.Add(this.radioButtonDownwards);
            this.groupBox2.Location = new System.Drawing.Point(135, 452);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(113, 48);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Dump Direction";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.textBoxEndAddress);
            this.groupBox3.Controls.Add(this.textBoxStartAddress);
            this.groupBox3.Location = new System.Drawing.Point(254, 452);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(288, 48);
            this.groupBox3.TabIndex = 8;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Search Area";
            // 
            // textBoxStartAddress
            // 
            this.textBoxStartAddress.Location = new System.Drawing.Point(42, 19);
            this.textBoxStartAddress.Name = "textBoxStartAddress";
            this.textBoxStartAddress.Size = new System.Drawing.Size(100, 20);
            this.textBoxStartAddress.TabIndex = 0;
            this.textBoxStartAddress.Text = "00000000";
            // 
            // textBoxEndAddress
            // 
            this.textBoxEndAddress.Location = new System.Drawing.Point(170, 19);
            this.textBoxEndAddress.Name = "textBoxEndAddress";
            this.textBoxEndAddress.Size = new System.Drawing.Size(100, 20);
            this.textBoxEndAddress.TabIndex = 1;
            this.textBoxEndAddress.Text = "1CCCCCCC";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "to";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "From";
            // 
            // SCSReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 524);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOpenFolder);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelBaseAddress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SCSReaderForm";
            this.ShowIcon = false;
            this.Text = "SCS Memory Scanner/Analyzer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelBaseAddress;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonOpenFolder;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBoxDumpSizeBytes;
        private System.Windows.Forms.RadioButton radioButtonUpwards;
        private System.Windows.Forms.RadioButton radioButtonDownwards;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEndAddress;
        private System.Windows.Forms.TextBox textBoxStartAddress;
    }
}

