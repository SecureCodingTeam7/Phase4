/*
 * SCS Memory Reader / Analyzer
 * Lecture: Secure Coding, Team 7, Phase 4
 * Chair XXII Software Engineering, Department of Computer Science TU München
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SecCodeSCSExploit
{
    /* Standard Implementation for reading process memory via C#, used by the MemoryScanner */
    public class ReadMem
    {
        #region Members
        // handle to the hooked process
        public int processHandle;
        #endregion

        #region DLL Imports
        [DllImport("kernel32.dll")]
        public static extern int OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesRead);

        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress, byte[] buffer, int size, int lpNumberOfBytesWritten);

        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);
        #endregion

        #region Methods
        public ReadMem( Process p )
        {
            uint DELETE = 0x00010000;
            uint READ_CONTROL = 0x00020000;
            uint WRITE_DAC = 0x00040000;
            uint WRITE_OWNER = 0x00080000;
            uint SYNCHRONIZE = 0x00100000;
            uint END = 0xFFF; // Change to 0xFFFF for WinXP/Server2003
            uint PROCESS_ALL_ACCESS = (DELETE |
                       READ_CONTROL |
                       WRITE_DAC |
                       WRITE_OWNER |
                       SYNCHRONIZE |
                       END
                     );

            this.processHandle = OpenProcess( PROCESS_ALL_ACCESS, false, p.Id );
        }

        public static byte[] ReadMemory( int adress, int processSize, int processHandle )
        {
            byte[] buffer = new byte[ processSize ];
            ReadProcessMemory( processHandle, adress, buffer, processSize, 0 );
            return buffer;
        }

        public static void WriteMemory( int adress, byte[] processBytes, int processHandle )
        {
            WriteProcessMemory( processHandle, adress, processBytes, processBytes.Length, 0 );
        }


        public static int GetObjectSize( object TestObject )
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            byte[] Array;
            bf.Serialize( ms, TestObject );
            Array = ms.ToArray();
            return Array.Length;
        }

        public bool CloseHandle()
        {
            try
            {
                int iRetValue;
                iRetValue = CloseHandle((IntPtr)this.processHandle);
                if (iRetValue == 0)
                {
                    throw new Exception("Failed to close process handle.");
                }

                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Warning", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
            return false;
        }
        #endregion
    }
}