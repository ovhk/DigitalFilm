using DigitalFilm.Papers;
using ImageMagick;
using System;
using System.Windows.Forms;

namespace DigitalFilm
{
    internal static class Program
    {

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
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
            
            // Initialize ImageMagik
            MagickNET.Initialize();

            // Start app
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

            // Close app
            Log.WriteLine("Closing!");
            Log.Close();
        }
    }
}
