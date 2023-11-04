using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalFilm.FFmpegWrapper
{
    internal class FFmpeg
    {
        /// <summary>
        /// FFmpeg exe path
        /// </summary>
        private const string FFmpegPath = @"ffmpeg\ffmpeg.exe";

        /// <summary>
        /// Execute FFmpeg to concat images into a single video 
        /// </summary>
        /// <param name="imagesPath"></param>
        /// <returns></returns>
        public static bool Execute(string imagesPath)
        {
            string result = "";

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = FFmpegPath;
                p.StartInfo.Arguments = "-i " + '"' + imagesPath + @"\" + FFconcat.InputFileName + '"' + " "+ '"' + imagesPath + @"\out.avi" + '"';
                p.Start();
                p.WaitForExit();

                // TODO : is there any parameter for compression?

                result = p.StandardOutput.ReadToEnd();

                // if nedded to chech duration : ffprobe out.avi -show_entries format=duration -v 0

                Log.WriteLine("FFmpeg result : " + result);
            }

            return true;
        }
    }
}
