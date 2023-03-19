﻿using System;
using System.Collections.Generic;
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
        public string Name => "Mode 2";

        public string Description => "";

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool IMode.Load(string[] imgPaths, int duration)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();
            GenerateMasquesTemps(engine.Width, engine.Height);

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

        private static void GenerateMasquesTemps(int width, int height)
        {
            DisplayEngine engine = DisplayEngine.GetInstance();

            Bitmap b = new Bitmap(width, height);

            using (Graphics gfx = Graphics.FromImage(b))
            {
                for (int i = 0; i < engine.NumberOfColors; i++)
                {
                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(engine.NumberOfColors - 1 - i, engine.NumberOfColors - 1 - i, engine.NumberOfColors - 1 - i)))
                    {
                        gfx.FillRectangle(brush, i * (width / engine.NumberOfColors), 0, width / engine.NumberOfColors, height / 2);
                    }
                }

                SolidBrush brushBlack = new SolidBrush(Color.Black);
                SolidBrush brushWhite = new SolidBrush(Color.White);

                int size = 40;

                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        SolidBrush brush = (j > i) ? brushBlack : brushWhite;
                        SolidBrush brushTxt = (j > i) ? brushWhite : brushBlack;

                        gfx.FillRectangle(brush, j * (width / size), height / 2, width / size, height / 2);

                        string str = (j + 1).ToString();

                        SizeF stringSize = new SizeF();
                        stringSize = gfx.MeasureString(str, SystemFonts.DefaultFont);
                        int offset = size / 2 - (int)stringSize.Width / 2;

                        gfx.DrawString(str, SystemFonts.DefaultFont, brushTxt, j * (width / size) + offset, height / 2 + 10);
                    }
                    engine.PushImage(new Bitmap(b), 500);
                }
            }
        }

    }
}