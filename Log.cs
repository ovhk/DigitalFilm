using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    /// <summary>
    /// File Log Manager
    /// </summary>
    internal class Log
    {
        private static StringBuilder sb = new StringBuilder();
        private static DateTime lastDT = DateTime.Now;
        private static string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private const int FLUSH_INTERVAL = 90; // in seconds
        private const string FILE_NAME = "log.txt";

        private static DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal static void Write(string text, params object[] parameters)
        {
            sb.Append(string.Format("{0} {1} : ", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString()));
            sb.Append(string.Format(text, parameters));

            if ((DateTime.Now - lastDT).TotalSeconds > FLUSH_INTERVAL && engine.Status != EngineStatus.Running)
            {
                System.IO.File.AppendAllText(filePath + "\\" + FILE_NAME, sb.ToString());
                sb.Clear();
                lastDT = DateTime.Now;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        internal static void WriteLine(string text, params object[] parameters)
        {
            Write(text + "\r\n", parameters);
        }

        /// <summary>
        /// 
        /// </summary>
        internal static void Close()
        {
            WriteLine("Closing!");
            System.IO.File.AppendAllText(filePath + "\\" + FILE_NAME, sb.ToString());
            sb.Clear();
        }
    }
}
;