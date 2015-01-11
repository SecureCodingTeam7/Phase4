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
using System.Windows.Forms;

namespace SecCodeSCSExploit
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SCSReaderForm());
        }
    }
}
