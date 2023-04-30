using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalFilm.Engine;

namespace DigitalFilm.Modes
{
    internal class Mode1 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Black & White";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Just a black & White zone to check if the diplay panel is able to block light.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration in second")]
        public int Duration
        { get; set; } = 10;

        /// <summary>
        /// Access to the Engine
        /// </summary>
        private readonly DisplayEngine engine = DisplayEngine.GetInstance();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load()
        {
            Bitmap b = new Bitmap(engine.Panel.Width, engine.Panel.Height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                // First, erase all
                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.White)) // will be black on the paper
                {
                    gfx.FillRectangle(brush, 0, 0, engine.Panel.Width / 2, engine.Panel.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.Black)) // will be white on the paper
                {
                    gfx.FillRectangle(brush, engine.Panel.Width / 2, 0, engine.Panel.Width / 2, engine.Panel.Height);
                }
            }

            engine.PushImage(b, Duration * 1000);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Unload()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            engine.Clear();

            return true;
        }

        /// <summary>
        /// Return the Name parameter
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
