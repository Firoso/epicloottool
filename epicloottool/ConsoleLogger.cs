using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epicloottool
{
    public static class ConsoleLogger
    {
        public static void Log(string m)
        {
            System.Console.ForegroundColor = ConsoleColor.Gray;
            System.Console.WriteLine(m);
        }

        public static void Warn(string m)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine(m);
        }

        public static void Error(string m)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(m);
        }
    }
}
