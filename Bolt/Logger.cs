using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bolt
{
    public static class Logger
    {
        static Logger()
        {
            //Setup log file
        }

        public static void Dispose()
        {
            //clean up log file
        }

        public static void Info(string message)
        {
            Print($"INFO: {message}");
        }

        public static void Warn(string message)
        {
            Print($"WARN: {message}");
        }

        public static void Error(string message, bool fatal)
        {
            Print($"ERROR: {message}");
            if (fatal)
            {
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        private static void Print(string message)
        {
            Console.WriteLine(message);
        }

        public static void Debug(string message, [CallerMemberName] string callerName = "", [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if(System.Diagnostics.Debugger.IsAttached)
            {
                string temp = $"{callerPath}:{callerName}@{callerLineNumber}: {message}";
                Print(temp);
                System.Diagnostics.Debug.WriteLine(temp);
            }            
        }
    }
}
