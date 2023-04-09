using DigitalDarkroom.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    internal class Mode7 : IMode
    {
        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Name => "Gray vs GrayToTime";

        // TODO : bug sur le temps, idem Mode 4 et 7 !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        /// <summary>
        /// 
        /// </summary>
        [Browsable(false)]
        public string Description => "";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Load()
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            int width = engine.Panel.Width;
            int height = engine.Panel.Height;

            int[] gttTimings = GrayToTime.GetTimings();

            SolidBrush brushBlack = new SolidBrush(Color.Black);
            SolidBrush brushWhite = new SolidBrush(Color.White);

            using (Bitmap b = new Bitmap(width, height))
            {
                using (Graphics gfx = Graphics.FromImage(b))
                {
                    int iWidth = (width / gttTimings.Length);

                    gfx.FillRectangle(brushWhite, 0, 0, width, height); // clear first // TODO ici, on montre que iWidth n'est pas bon... il manque +1 ou -1 sur 255 couleur

                    for (int i = 0; i < gttTimings.Length; i++)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i, engine.Panel.NumberOfColors - 1 - i)))
                        {
                            gfx.FillRectangle(brush, i * iWidth, 0, iWidth, height / 2);
                        }
                    }

                    for (int i = 0; i < gttTimings.Length; i++)
                    {
                        if (i == 1)
                        {
                            //gfx.FillRectangle(brushBlack, 0, 0, width, height / 2);
                        }

                       // gfx.FillRectangle(brushBlack, 0, height / 2, width, height / 2);

                        // TODO bizarrement, on ne va pas jusqu'au bout : bug sur les mignatures ?????? A l'affichage ????
                        gfx.FillRectangle(brushWhite, i * iWidth, height / 2, iWidth, height / 2);

                        //TODO : !!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                        // new Bitmap because we need a copy, next iteration b will be changed
                        engine.PushImage(new Bitmap(b), gttTimings[i]);
                    }
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
