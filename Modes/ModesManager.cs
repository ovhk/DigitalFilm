using DigitalFilm.Modes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Modes
{
    public class ModesManager
    {
        /// <summary>
        /// List of papers
        /// Add new mode here !
        /// </summary>
        public static List<IMode> Modes = new List<IMode>
        {
            // Add here new mode Class!

            new Modes.Mode1(),
            //new Modes.Mode2(),  // Too difficult de compare gray visually
            new Modes.Mode3(),
            //new Modes.Mode4(), // Unuseful to compare with a linear gray
            new Modes.Mode5(),
            new Modes.Mode50(),
            new Modes.Mode6(),
            //new Modes.Mode7(), // Unuseful
            new Modes.Mode8(),
            new Modes.Mode9(),
            new Modes.Mode10(),
        };
    }
}
