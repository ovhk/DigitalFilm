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
        static void Main()
        {
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
