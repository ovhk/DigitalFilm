using DigitalFilm.Tools;
using System;
using System.IO;
using System.Reflection;

namespace DigitalFilm.Engine
{
    public class CacheManager
    {
        /// <summary>
        /// Access to DisplayEngine
        /// </summary>
        private readonly DisplayEngine engine;

        /// <summary>
        /// 
        /// </summary>
        public CacheManager(DisplayEngine engine)
        {
            this.engine = engine;
            _ = Directory.CreateDirectory(this.tmpCacheDir);
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
        private static readonly string appDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// 
        /// </summary>
        private readonly string cacheDir = appDir + @"\cache\";

        /// <summary>
        /// 
        /// </summary>
        private readonly string tmpCacheDir = appDir + @"\cache\tmp";

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
                string[] list = Directory.GetFiles(cachedir, CacheManager.ExtentionSearchFilter);

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

            _ = Directory.CreateDirectory(this._cachePath);
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
        /// <param name="exposureTime"></param>
        /// <returns></returns>
        public string GetTmpCachePath(int index, int exposureTime)
        {
            return this._cachePath != null
                ? this._cachePath + @"\" + index + "_" + exposureTime + Extention
                : tmpCacheDir + @"\" + index + "_" + exposureTime + Extention;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="index"></param>
        /// <param name="exposureTime"></param>
        /// <returns></returns>
        public bool GetIndexAndExposureTime(string fileName, out int index, out int exposureTime)
        {
            try
            {
                string a = fileName.Split('.')[0]; // remove extention

                string[] b = a.Split('_');

                index = int.Parse(b[0]);
                exposureTime = int.Parse(b[1]);
            }
            catch
            {
                index = 0;
                exposureTime = 0;
                return false;
            }

            return true;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public TimeSpan GetTotalExposureTime()
        //{
        //    TimeSpan tsTotalExposureTime = TimeSpan.Zero;

        //    string[] fileEntries = this.GetFiles();

        //    foreach (string fileName in fileEntries)
        //    {
        //        FileInfo file = new FileInfo(fileName);

        //        GetIndexAndExposureTime(file.Name, out int index, out int exposureTime);

        //        tsTotalExposureTime += TimeSpan.FromMilliseconds(exposureTime);
        //    }

        //    return tsTotalExposureTime;
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

            if (fileEntries == null || fileEntries.Length == 0)
            {
                return;
            }

            foreach (string fileName in fileEntries)
            {
                File.Delete(fileName);
            }
        }
    }
}
