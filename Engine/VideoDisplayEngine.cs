using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Engine
{
    public class VideoDisplayEngine
    {
        public LibVLC _libVLC;
        public MediaPlayer _mp;

        #region Singleton & Constructor

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static VideoDisplayEngine _instance;

        /// <summary>
        /// Lock object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Get the instance of the singleton
        /// </summary>
        /// <returns></returns>
        public static VideoDisplayEngine GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new VideoDisplayEngine();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Display Engine private constructor, use only the GetInstance
        /// </summary>
        private VideoDisplayEngine()
        {
            _libVLC = new LibVLC();
            _mp = new MediaPlayer(_libVLC);
        }

        #endregion

    }
}
