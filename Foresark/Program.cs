using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Foresark
{
    class Program
    {
        static void Main(string[] args)
        {
            Output.printFullLine(ConsoleColor.Red);
            Output.printLine("FORESARK FORSAKEN v" + getVersion(), ConsoleColor.Red, ConsoleColor.Black, true);
            Output.printLine("Powered by: boasfesta", ConsoleColor.Yellow, ConsoleColor.Black, true);
            Output.printFullLine(ConsoleColor.Red);
            Output.newLine();
            Output.printLine("Press any key to continue...");

            Output.pause();
            new Foresark();
        }

        public static string getVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi.FileVersion;
        }
    }
}
