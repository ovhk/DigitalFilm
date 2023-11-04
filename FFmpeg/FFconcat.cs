using DigitalFilm.Engine;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.FFmpegWrapper
{
    internal class FFconcat
    {
        public const string InputFileName = "input.ffconcat.txt";

        // ffconcat tags
        private const string HeaderTag = "ffconcat version 1.0";
        private const string FileTag = "file ";
        private const string DurationTag = "duration ";

        /// <summary>
        /// Generate ffconcat file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="inputFiles"></param>
        /// <returns></returns>
        public static bool Generate(string path, List<ImageLayer> inputFiles)
        {
            string filepath = Path.Combine(path, InputFileName);

            if (File.Exists(filepath)) 
            {
                File.Delete(filepath);
            }

            using (StreamWriter sw = File.CreateText(filepath))
            {
                sw.WriteLine(HeaderTag);

                foreach (ImageLayer il in inputFiles)
                {
                    // "file file.img"
                    sw.Write(FileTag);
                    string filename = Path.GetFileName(il.ImagePath);
                    sw.WriteLine(filename);

                    // "duration n.nnn"
                    sw.Write(DurationTag);
                    double duration = il.ExposureTime / 1000.0; // from ms to second
                    sw.WriteLine(duration.ToString("F2", CultureInfo.CreateSpecificCulture("en-US")));
                }
            }

            return true;
        }
    }
}
