using DigitalFilm.Papers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DigitalFilm
{
    internal static class Program
    {

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if GENERATE_PAPER_DATA
            {
                List<Paper> papers = new List<Paper>
                                    {
                                        new FomaspeedVariantIIIGlossyG0(),
                                        new FomaspeedVariantIIIVelvetG0(),
                                        new FomaspeedVariantIIIMattG0(),
                                        new FomaspeedVariantIIIGlossyG1(),
                                        new FomaspeedVariantIIIGlossyG2(),
                                        new FomaspeedVariantIIIGlossyG3(),
                                        new FomaspeedVariantIIIGlossyG4(),
                                        new FomaspeedVariantIIIGlossyG5()
                                    };

                //foreach (Paper p in papers)
                //{
                //    Console.Write(p);
                //}

                //Log.WriteLine(";I;G0;G1;G2;G3;G4;G5;IG0;IG1;IG2;IG3;IG4;IG5;0.7;1/0.7;1.4");

                //for (int i = 0; i < papers[0].Data.Length; i++)
                //{

                //}

                Paper g0 = new FomaspeedVariantIIIGlossyG0();
                Paper g1 = new FomaspeedVariantIIIGlossyG1();
                Paper g2 = new FomaspeedVariantIIIGlossyG2();
                Paper g3 = new FomaspeedVariantIIIGlossyG3();
                Paper g4 = new FomaspeedVariantIIIGlossyG4();
                Paper g5 = new FomaspeedVariantIIIGlossyG5();

                Log.WriteLine(";I;G0;G1;G2;G3;G4;G5;IG0;IG1;IG2;IG3;IG4;IG5;0.7;1/0.7;1.4");

                for (int i = 0; i < g0.Data.Length; i++)
                {
                    Log.WriteLine(";{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12};{13};{14};{15}",
                        i,
                        g0.Data[i],
                        g1.Data[i],
                        g2.Data[i],
                        g3.Data[i],
                        g4.Data[i],
                        g5.Data[i],
                        g0.InvertedData[i],
                        g1.InvertedData[i],
                        g2.InvertedData[i],
                        g3.InvertedData[i],
                        g4.InvertedData[i],
                        g5.InvertedData[i],
                        1d * Math.Pow((255d - (double)i) / 255d, 0.7) * 255d,
                        1d * Math.Pow((255d - (double)i) / 255d, 1d / 0.7) * 255d,
                        1d * Math.Pow((255d - (double)i) / 255d, 1.4) * 255d
                        );
                }
                Log.Close();
                return;
            }
#endif
            Log.WriteLine("######################################################");
            Log.WriteLine("Starting");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
            Log.WriteLine("Closing!");
            Log.Close();
        }
    }
}
