using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalDarkroom.Engine;
using DigitalDarkroom.Tools;

namespace DigitalDarkroom.Modes
{
    internal class Mode2 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Find duration for a gray";

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "Find the duration for a specific gray in parameter.";

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Gray value from 0 to 255")]
        public int GrayValue
        { get; set; } = 100;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration")]
        public int NbInterval
        { get; set; } = 30;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Display duration in ms")]
        public int IntervalDuration
        { get; set; } = 2000;

        /// <summary>
        /// 
        /// </summary>
        [Category("Configuration")]
        [Description("Initial duration in ms")]
        public int InitialDuration
        { get; set; } = 0;

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
            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / NbInterval);

            // width / NbInterval not round so we adjust...
            width = iWidth * NbInterval;

            Bitmap b = new Bitmap(width, height);

            Graphics gfx = Graphics.FromImage(b);

            // First, erase all
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                gfx.FillRectangle(brush, 0, 0, engine.Panel.Width, engine.Panel.Height);
            }

            // Generate gray levels
            using (SolidBrush brush = new SolidBrush(ColorTools.GetInvertedColor(GrayValue)))
            {
                gfx.FillRectangle(brush, 0, 0, width, height / 2);
            }

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            SolidBrush brushTxt = (GrayValue < 256 / 2) ? brushBlack : brushWhite;

            DrawTools.DrawLargestString(ref gfx, ref brushTxt, "GRAY : " + GrayValue, new Rectangle(0, 0, width, height / 2));

            if (InitialDuration > 0)
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    gfx.FillRectangle(brush, 0, height / 2, width, height / 2);
                }

                engine.PushImage(new Bitmap(b), InitialDuration);

                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    gfx.FillRectangle(brush, 0, height / 2, width, height / 2);
                }
            }

            for (int i = 0; i < NbInterval; i++)
            {
                for (int j = 0; j < NbInterval; j++)
                {
                    SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                    brushTxt = (j > i) ? brushWhite : brushBlack;

                    gfx.FillRectangle(brush, j * iWidth, height / 2, iWidth, height / 2);

                    // TODO formule compliqué sans doute pour rien...
                    string str = (InitialDuration + NbInterval * IntervalDuration - j * IntervalDuration).ToString();

                    DrawTools.DrawLargestString(ref gfx, ref brushTxt, str, new Rectangle(j * iWidth, height/2, iWidth, height / 2));
                }
                engine.PushImage(new Bitmap(b), IntervalDuration);
            }

            gfx.Dispose();

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Unload()
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
