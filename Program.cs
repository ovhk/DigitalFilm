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
            string[] args = Environment.GetCommandLineArgs();

            PapersManager papersManager = PapersManager.GetInstance();

            papersManager.Load();

            if (args.Length > 1 && args?[1] == "/papersdata")
            {
                Log.WriteLine("++++++++++++++++++++++++++++++++++++++++++++++++++++++");
                Log.WriteLine("Generate Papers Data");

                papersManager.Save(Properties.Settings.Default.PapersDataFileName);

                Log.WriteLine("Closing!");
                Log.Close();
                return;
            }

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
