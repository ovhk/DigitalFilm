using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [Description("Display duration in second")]
        public int IntervalDuration
        { get; set; } = 2;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;
            int iWidth = (width / NbInterval);

            // width / NbInterval not round so we adjust...
            width = iWidth * NbInterval;

            Bitmap b = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                // Generate gray levels
                using (SolidBrush brush = new SolidBrush(Color.FromArgb(GrayValue, GrayValue, GrayValue)))
                {
                    gfx.FillRectangle(brush, 0, 0, width, height / 2);
                }

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                SolidBrush brushTxt = (GrayValue > 256/2) ? brushBlack : brushWhite;

                Font font = new Font("Microsoft Sans Serif", 20F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

                gfx.DrawString("GRAY : " + GrayValue, font, brushTxt, width/2, height/4);

                for (int i = 0; i < NbInterval; i++)
                {
                    for (int j = 0; j < NbInterval; j++)
                    {
                        SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                        brushTxt = (j > i) ? brushWhite : brushBlack;

                        gfx.FillRectangle(brush, j * iWidth, height/2, iWidth, height/2);

                        // TODO formule compliqué sans doute pour rien...
                        string str = NbInterval * IntervalDuration - (j) * IntervalDuration + "\r\nsec.";

                        SizeF stringSize = new SizeF();
                        stringSize = gfx.MeasureString(str, SystemFonts.DefaultFont);
                        int offset = NbInterval / 2 - (int)stringSize.Width / 2 + 10;

                        gfx.DrawString(str, SystemFonts.DefaultFont, brushTxt, j * iWidth + offset, height - 40);
                    }
                    engine.PushImage(new Bitmap(b), IntervalDuration * 1000);
                }
            }
            
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
