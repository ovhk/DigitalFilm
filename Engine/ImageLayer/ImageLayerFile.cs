using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Engine;

namespace DigitalDarkroom.Engine
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
#if USE_CACHE
            DisplayEngine engine = DisplayEngine.GetInstance();

            if (engine._cachePath != null)
                return engine._cachePath + @"\" + index + "_" + expositionDuration + Extention;
            
            return FilesPath + @"\" + index + "_" + expositionDuration + Extention; // TODO : change cache location ... in cache dit
#else
            return FilesPath + @"\" + index + "_" + expositionDuration + Extention;
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string[] GetFiles()
        {
            // TODO : avec cache, cela ne marche plus
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
                File.Delete(fileName);
            } 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <param name="expositionDuration"></param>
        /// <returns></returns>
        public static bool GetIndexAndExpositionDuration(string fileName, out int index, out int expositionDuration)
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
                index = 0; 
                expositionDuration = 0;
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

                GetIndexAndExpositionDuration(file.Name, out int index, out int expositionDuration);

                tsTotalDuration += TimeSpan.FromMilliseconds(expositionDuration);
            }

            return tsTotalDuration;
        }
    }
}
