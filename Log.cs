using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    internal class Log
    {
        internal static void Write(string text)
        {

        }

        internal static void WriteLine(string text)
        {
            Write(text + "\r\n");
        }
    }
}
