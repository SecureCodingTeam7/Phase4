/* 
 * SCS Memory Reader / Analyzer
 * Lecture: Secure Coding, Team 7, Phase 4
 * Chair XXII Software Engineering, Department of Computer Science TU München
 *
 * Author: Elias Tatros
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace SecCodeSCSExploit
{
    /* The SCSReaderForm handles the user interface of the application.
     * It is responsible for "hooking" the target process and starting the SCSMemoryReader as a separate thread.
     * 
     * It also subscribes to the MemoryReaders message & progress events, which are then relayed to the user,
     * via the form controls.
     */
    public partial class SCSReaderForm : Form
    {
        #region Members
        SCSMemoryReader SCSMemReader;
        Thread t;
        private int progress;

        private int dumpSizeBytes;
        private int addressBegin;
        private int addressEnd;
        private bool upwardsDump;
        #endregion

        #region Constructor
        public SCSReaderForm()
        {
            InitializeComponent();

            // Initialize Form Controls depending on Settings
            this.textBoxDumpSizeBytes.Text = Settings.dumpSizeBytes.ToString();
            this.textBoxStartAddress.Text = Settings.addressBegin;
            this.textBoxEndAddress.Text = Settings.addressEnd;
            this.radioButtonUpwards.Checked = Settings.upwardsDump;
            this.radioButtonDownwards.Checked = !Settings.upwardsDump;
        }
        #endregion

        #region Methods
        // Validate form input
        public bool validateInput()
        {
            // START ADDRESS VALIDATION
            // Check Start Address Empty
            if (this.textBoxStartAddress.Text == "")
            {
                MessageBox.Show("Start Address can not be empty.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // Parse Start address
                int startAddressTemp = int.Parse(this.textBoxStartAddress.Text, System.Globalization.NumberStyles.HexNumber);

                // Check Start Address in range
                if (startAddressTemp == -1)
                {
                    MessageBox.Show("Start Address must be a value between '0' and '7FFFFFFF'", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // Set Start Address
                this.addressBegin = startAddressTemp;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Start Address must be a value between '0' and '7FFFFFFF'", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }



            // END ADDRESS VALIDATION
            // Check End Address Empty
            if (this.textBoxEndAddress.Text == "")
            {
                MessageBox.Show("End Address can not be empty.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // Parse End address
                int endAddressTemp = int.Parse(this.textBoxEndAddress.Text, System.Globalization.NumberStyles.HexNumber);
                // Check End Address in range
                if (endAddressTemp == -1)
                {
                    MessageBox.Show("End Address must be a value between '0' and '7FFFFFFF'", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // Set End Address
                this.addressEnd = endAddressTemp;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("End Address must be a value between '0' and '7FFFFFFF'", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }


            // MEMORY AREA VALIDATION
            // Check memory area dump size Empty
            if (this.textBoxDumpSizeBytes.Text == "")
            {
                MessageBox.Show("Memory area dump size can not be empty.", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }

            try
            {
                // Parse memory area dump size
                int dumpSizeBytesTemp = int.Parse(this.textBoxDumpSizeBytes.Text);

                // Check memory area dump size in range
                if (dumpSizeBytesTemp > 10000000 || dumpSizeBytesTemp < 0)
                {
                    MessageBox.Show("Memory area dump size must be a value between '0' and '10000000'", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                    return false;
                }

                // Set memory area size
                this.dumpSizeBytes = dumpSizeBytesTemp;
            }
            catch (System.FormatException)
            {
                MessageBox.Show("Memory area dump size must be a value between '0' and '10000000'", "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
                return false;
            }



            // Set dump direction
            this.upwardsDump = this.radioButtonUpwards.Checked;

            return true;
        }

        private void updateProgress()
        {
            if (progressBar1.InvokeRequired)
                progressBar1.Invoke(new Action(updateProgress));
            else
                progressBar1.Value = this.progress;
        }

        private void updateTextbox(String message, bool newLine)
        {
            if (newLine)
                message += "\r\n";

            if (this.textBox1.InvokeRequired)
                textBox1.Invoke(new Action (() => this.textBox1.Text = this.textBox1.Text + message + "\n"));
            else
                this.textBox1.Text = this.textBox1.Text + message;
        }

        private bool hookProcess(String processName)
        {
            // Optional: Do clean up
            if (SCSMemReader != null)
            {
                // Terminate the working thread
                if (t != null)
                {
                    t.Abort();
                }

                // Clear previous output
                this.textBox1.Clear();

                // Close process handle
                if (this.SCSMemReader.closeHandle())
                    this.textBox1.Text = this.textBox1.Text + "Successfully closed existing process handle.\r\n";
                else
                    this.textBox1.Text = this.textBox1.Text + "Unable to close existing process handle.\r\n";
            }

            if (validateInput())
            {
                // Create a new Memory Reader
                this.SCSMemReader = new SCSMemoryReader(this.addressBegin, this.addressEnd, this.dumpSizeBytes, this.upwardsDump);

                // Hook the specified process and obtain base address
                if (SCSMemReader.hookProcess(Settings.processName))
                {
                    this.textBox1.Text = this.textBox1.Text + "Successfully hooked process \"" + Settings.processName + "\"\r\n";
                    this.labelBaseAddress.Text = "baseAddress: " + SCSMemReader.basePtr.ToString("X");
                    return true;
                }

                else
                {
                    this.textBox1.Text = this.textBox1.Text + "Failed to hook process " + Settings.processName + "\r\n";
                    return false;
                }
            }
            return false;
        }

        private void scanMemory()
        {
            // Subscribe to progress change events
            SCSMemReader.ProgressChanged += new SCSMemoryReader.ProgressChangedEventHandler(SCSMemReader_ProgressChanged);
            // Subscribe to message events
            SCSMemReader.MessagePending += new SCSMemoryReader.MessagePendingEventHandler(SCSMemReader_MessagePending);

            t = new Thread(new ThreadStart(SCSMemReader.scanMemory));
            t.Start();
        }
        #endregion

        #region Event Callback Methods
        private void button1_Click(object sender, EventArgs e)
        {
            if (hookProcess(Settings.processName))
            {
                scanMemory();
            }
        }

        private void SCSMemReader_ProgressChanged(object sender, MemoryScanner.ProgressChangedEventArgs e)
        {
            this.progress = e.Progress;
            updateProgress();
        }

        private void SCSMemReader_MessagePending(object sender, MemoryScanner.MessagePendingEventArgs e)
        {
            updateTextbox(e.Message, e.NewLine);
        }

        private void buttonOpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath);
        }
        #endregion
    }
}
