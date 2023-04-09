using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom
{
    /// <summary>
    /// 
    /// </summary>
    public class ImageLayerFile
    {
        /// <summary>
        /// 
        /// </summary>
        public static string Extention => ".bmp";

        /// <summary>
        /// 
        /// </summary>
        public static string ExtentionSearchFilter => "*" + Extention;

        /// <summary>
        /// 
        /// </summary>
        public static string FilesPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // TODO rendre le chemin paramètrable dans le fichier de conf avec une valeur par défaut

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="expositionDuration"></param>
        /// <returns></returns>
        public static string GetImagePath(int index, int expositionDuration)
        {
            return FilesPath + @"\" + index + "_" + expositionDuration + Extention;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetFiles()
        {
            return Directory.GetFiles(ImageLayerFile.FilesPath, ImageLayerFile.ExtentionSearchFilter);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void ClearFiles()
        {
            string[] fileEntries = ImageLayerFile.GetFiles();

            foreach (string fileName in fileEntries)
            {
                System.IO.File.Delete(fileName);
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <param name="expositionDuration"></param>
        /// <returns></returns>
        public static bool GetIndexAndExpositionDuration(string fileName, ref int index, ref int expositionDuration)         // TODO use out instead of ref ?
        {
            try
            {
                string a = fileName.Split('.')[0]; // remove extention

                string[] b = a.Split('_');

                index = int.Parse(b[0]);
                expositionDuration = int.Parse(b[1]);
            }
            catch 
            { 
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TimeSpan GetTotalDuration()
        {
            TimeSpan tsTotalDuration = TimeSpan.Zero;

            string[] fileEntries = ImageLayerFile.GetFiles();

            foreach (string fileName in fileEntries)
            {
                FileInfo file = new FileInfo(fileName);

                int index = 0;
                int expositionDuration = 0;

                GetIndexAndExpositionDuration(file.Name, ref index, ref expositionDuration);

                tsTotalDuration += TimeSpan.FromMilliseconds(expositionDuration);
            }

            return tsTotalDuration;
        }
    }
}
