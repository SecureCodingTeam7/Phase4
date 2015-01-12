/* 
 * SCS Memory Reader / Analyzer
 * Lecture: Secure Coding, Team 7, Phase 4
 * Chair XXII Software Engineering, Department of Computer Science TU München
 *
 * Author: Elias Tatros
 * This class is inspired by the article "How to write a Memory Scanner", Rojan Gh., on codeproject.com.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecCodeSCSExploit
{
    /* High-level implementation of the memory scanner. Searches a specified value in memory using the low-level ReadMem API.
     * Addresses that contained the search value are stored in a list.
     * Currently only supports 8 byte values (Int64), which in our case is sufficient to identify memory
     * locations of the static IV (or other interesting Strings/data).
     */
    public class MemoryScanner
    {
        #region Members
        // Memory Reader used
        ReadMem reader;

        // The value we are searching in memory
        Int64 searchValue;

        // Size of the search value in bytes
        int searchValueBytesCount;

        // Size of search value in bytes
        // minus the one byte we are moving with each scan
        int scanDifference;

        // List of addresses that contained the search value
        List<int> foundAddresses;

        // The point in memory we start scanning at
        IntPtr lastAddress;
        // The point in memory where we stop scanning
        IntPtr baseAddress;
        // The size of the memory area we are scanning
        int scanAreaSize;

        // The size of the memory blocks read in one interation in bytes
        int readBufferSize;
        #endregion

        #region Delegates & Events
        // progress changed delegate
        public delegate void ProgressChangedEventHandler(object sender, ProgressChangedEventArgs e);
        // progress changed event
        public event ProgressChangedEventHandler ProgressChanged;

        // Message pending delegate
        public delegate void MessagePendingEventHandler(object sender, MessagePendingEventArgs e);
        // Message pending event
        public event MessagePendingEventHandler MessagePending;
        #endregion

        #region Event Args
        // progress changed event args
        public class ProgressChangedEventArgs : EventArgs
        {
            private int progress;

            public ProgressChangedEventArgs(int progress)
            {
                this.progress = progress;
            }

            public int Progress
            {
                get
                {
                    return progress;
                }

                set
                {
                    progress = value;
                }
            }
        }

        // Message pending event args
        public class MessagePendingEventArgs : EventArgs
        {
            private String message;
            private bool newLine;

            public MessagePendingEventArgs(String message, bool newLine)
            {
                this.message = message;
                this.newLine = newLine;
            }

            public bool NewLine
            {
                get
                {
                    return this.newLine;
                }

                set
                {
                    this.newLine = value;
                }
            }

            public String Message
            {
                get
                {
                    return this.message;
                }

                set
                {
                    this.message = value;
                }
            }
        }
        #endregion

        #region Constructor
        public MemoryScanner(ReadMem reader, IntPtr baseAddress, IntPtr lastAddress, Int64 searchValue)
        {
            this.searchValue = searchValue;
            this.searchValueBytesCount = 8;
            this.scanDifference = searchValueBytesCount - 1;
            this.foundAddresses = new List<int>();
            this.baseAddress = baseAddress;
            this.lastAddress = lastAddress;
            this.scanAreaSize = (int) ((int)this.lastAddress - (int)this.baseAddress);
            this.readBufferSize = 20480;
            this.reader = reader;
        }
        #endregion

        #region Accessors
        public List<int> FoundAddresses
        {
            get
            {
                return this.foundAddresses;
            }
        }
        #endregion

        #region Methods
        private void findInt64InArray(int currentAddress, byte[] memoryContents, Int64 searchValue)
        {
            if (memoryContents.Length > 0)
            {
                for (int j = 0; j < memoryContents.Length - 7; j++)
                {
                    if (BitConverter.ToInt64(memoryContents, j) == searchValue)
                    {
                        foundAddresses.Add(currentAddress + j);
                        MessagePending(this, new MessagePendingEventArgs("Search Value (" + searchValue + ") found at: " + (currentAddress + j).ToString("X"), true));
                    }
                }
            }
        }

        public void updateProgress(int percentComplete)
        {
            // New Event Args for progress tracking
            ProgressChangedEventArgs progressChangedEventArgs = new ProgressChangedEventArgs(percentComplete);            

            // raise the event
            ProgressChanged(this, progressChangedEventArgs);
        }

        public void scan()
        {
            MessagePending(this, new MessagePendingEventArgs("Search Value: " + searchValue, true));
            MessagePending(this, new MessagePendingEventArgs("Search Value ASCII: " + Encoding.ASCII.GetString(BitConverter.GetBytes(searchValue)), true));
            MessagePending(this, new MessagePendingEventArgs("Start Address: " + baseAddress.ToString("X"), true));
            MessagePending(this, new MessagePendingEventArgs("Last Adddress: " + lastAddress.ToString("X"), true));
            MessagePending(this, new MessagePendingEventArgs("Search Area: " + ((int)lastAddress - (int)baseAddress).ToString("X"), true));

            // If the scan area is greater than the read buffer -> iterate
            if (scanAreaSize > readBufferSize)
            {
                // Number of iterations required
                int numIterations = scanAreaSize / readBufferSize;
                
                // Remainder in bytes
                int remainingBytes = scanAreaSize % readBufferSize;

                // Set current address to base address
                int currentAddress = (int)baseAddress;

                // Number of bytes read
                // int bytesReadCount;

                // Number of bytes to read
                int bytesToRead = readBufferSize;

                // Byte array of memory contents read
                byte[] memoryContents;

                // Percentage of addresses read / memory scanned
                int progress;

                // Iterate: Read memory blocks of length readBufferSize into memoryContents
                for (int i = 0; i < numIterations; i++)
                {
                    // Update percentage of addresses read
                    progress = (int) (((double) (currentAddress - (int) baseAddress) / (double) scanAreaSize) * 100d);
                    updateProgress(progress);

                    memoryContents = ReadMem.ReadMemory(currentAddress, bytesToRead, reader.processHandle);

                    findInt64InArray(currentAddress, memoryContents, searchValue);

                    // Increase currentAddress by the amount of bytes read minus the scanDifference.
                    currentAddress += memoryContents.Length - scanDifference;

                    // Increase the size of the readBuffer to account for the skipped bytes
                    bytesToRead = readBufferSize + scanDifference;
                }

                // If there are additional bytes smaller than the readBufferSize, read them as well and look
                // for our search value
                if (remainingBytes > 0)
                {
                    bytesToRead = (int)lastAddress - currentAddress;
                    memoryContents = ReadMem.ReadMemory(currentAddress, bytesToRead, reader.processHandle);
                    
                    findInt64InArray(currentAddress, memoryContents, searchValue);

                }
            }
            // If the block could be read in just one read,
            else
            {
                //Calculate the memory block's size.
                int blockSize = scanAreaSize % readBufferSize;

                //Set the currentAddress to first address.
                int currentAddress = (int)baseAddress;

                //If the memory block can contain at least one 64 bit variable.
                if (blockSize > 8)
                {
                    //Read the bytes to the array.
                    byte[] memoryContents = ReadMem.ReadMemory(currentAddress, blockSize, reader.processHandle);
                    findInt64InArray(currentAddress, memoryContents, searchValue);
                }
            }

            updateProgress(100);
            MessagePending(this, new MessagePendingEventArgs("Scan finished. Found: " + foundAddresses.Count + " instances of the search value (" + searchValue + ").", true));
            MessagePending(this, new MessagePendingEventArgs("  List of addresses: ", true));
            foreach (int address in foundAddresses)
            {
                MessagePending(this, new MessagePendingEventArgs("    Address: " + address.ToString("X"), true));
            }
            //Close the handle to the process to avoid process errors.
            // reader.CloseHandle();

        } // End of Scan Method
        #endregion
    } // End of Memory Scanner Class
}
