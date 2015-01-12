/* 
 * Lecture: Secure Coding, Team 7, Phase 4
 * Chair XXII Software Engineering, Department of Computer Science TU München
 *
 * Author: Elias Tatros
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecCodeSCSExploit
{
    class Settings
    {
        public static String processName = "javaw";
        // Starting Address
        public static String addressBegin = "00000000";
        // Ending Address
        public static String addressEnd = "1CCCCCCF";
        // Direction of dump
        public static bool upwardsDump = true; // true = upwards, false = downwards
        // Size of dump area in bytes
        public static int dumpSizeBytes = 1024;
    }
}
