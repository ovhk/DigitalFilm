using DigitalFilm.Tools;
using System.Collections.Generic;

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
            foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                if (screen.Primary == false)
                {
                    // search for known name
                    string name = ScreenInterrogatory.DeviceFriendlyName(screen);

                    switch (name)
                    {
                        case Wisecoco8k103Panel.Identification:
                            result.Add(new Panels.Wisecoco8k103Panel(screen));
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
