using DigitalDarkroom.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Engine
{
    public class CacheManager
    {
        /// <summary>
        /// Access to DisplayEngine
        /// </summary>
        private DisplayEngine engine;

        /// <summary>
        /// 
        /// </summary>
        public CacheManager(DisplayEngine engine) 
        {
            this.engine = engine;
            Directory.CreateDirectory(this.tmpCacheDir);
        }

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
        private static string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// 
        /// </summary>
        private string cacheDir = appDir + @"\cache\";

        /// <summary>
        /// 
        /// </summary>
        private string tmpCacheDir = appDir + @"\cache\tmp";

        /// <summary>
        /// 
        /// </summary>
        private string _cachePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        private string _formatCachePath(string identifier)
        {
            return cacheDir + identifier + @"\" + this.engine.Panel.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool IsInCache(string identifier)
        {
            return Directory.Exists(this._formatCachePath(identifier));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public bool LoadFromCache(string identifier)
        {
            string cachedir = this._formatCachePath(identifier);

            if (Directory.Exists(cachedir) == true)
            {
                var list = Directory.GetFiles(cachedir, CacheManager.ExtentionSearchFilter);

                Array.Sort(list, new AlphanumComparatorFast());

                foreach (string file in list)
                {
                    this.engine.PushImage(new ImageLayer(file));
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        public void SetCacheIdentifier(string identifier)
        {
            this._cachePath = this._formatCachePath(identifier);

            Directory.CreateDirectory(this._cachePath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        public void ClearCacheFromIdentifier(string identifier)
        {
            this._cachePath = this._formatCachePath(identifier);

            try
            {
                Directory.Delete(this._cachePath, true);
            }
            catch { }

            this._cachePath = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="expositionDuration"></param>
        /// <returns></returns>
        public string GetTmpCachePath(int index, int expositionDuration)
        {
            if (this._cachePath != null)
                return this._cachePath + @"\" + index + "_" + expositionDuration + Extention;

            return tmpCacheDir + @"\" + index + "_" + expositionDuration + Extention;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <param name="expositionDuration"></param>
        /// <returns></returns>
        public bool GetIndexAndExpositionDuration(string fileName, out int index, out int expositionDuration)
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public TimeSpan GetTotalDuration()
        //{
        //    TimeSpan tsTotalDuration = TimeSpan.Zero;

        //    string[] fileEntries = this.GetFiles();

        //    foreach (string fileName in fileEntries)
        //    {
        //        FileInfo file = new FileInfo(fileName);

        //        GetIndexAndExpositionDuration(file.Name, out int index, out int expositionDuration);

        //        tsTotalDuration += TimeSpan.FromMilliseconds(expositionDuration);
        //    }

        //    return tsTotalDuration;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string[] GetFiles()
        {
            try
            {
                return Directory.GetFiles(this.tmpCacheDir, CacheManager.ExtentionSearchFilter);
            }
            catch { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearTmpCache()
        {
            string[] fileEntries = this.GetFiles();

            if (fileEntries == null || fileEntries.Length == 0) return;

            foreach (string fileName in fileEntries)
            {
                File.Delete(fileName);
            }
        }
    }
}
