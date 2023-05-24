﻿using DigitalFilm.Papers;
using DigitalFilm.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Panels
{
    public class ScreenManager
    {
        #region Singleton & Constructor

        /// <summary>
        /// Singleton instance
        /// </summary>
        private static ScreenManager _instance;

        /// <summary>
        /// Lock object
        /// </summary>
        private static readonly object _lock = new object();

        /// <summary>
        /// Get the instance of the singleton
        /// </summary>
        /// <returns></returns>
        public static ScreenManager GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ScreenManager();
                    }
                }
            }
            return _instance;
        }

        /// <summary>
        /// Screen Manager private constructor, use only the GetInstance
        /// </summary>
        private ScreenManager()
        {

        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IPanel[] Detect()
        {
            List<IPanel> result = new List<IPanel>();

            // Find first external screen
            foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    // search for known name
                    var name = ScreenInterrogatory.DeviceFriendlyName(screen);

                    switch(name)
                    {
                        case "lontium semi":
                            result.Add(new Panels.Wisecoco8k103(screen));
                            break;
                        default:
                            result.Add(new Panels.ExternalPanel(screen));
                            break;
                    }

                    
                }
            }

            return result.ToArray();
        }
    }
}