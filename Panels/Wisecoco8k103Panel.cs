using DigitalFilm.Tools;
using System.Windows.Forms;

namespace DigitalFilm.Panels
{
    internal class Wisecoco8k103Panel : IPanel
    {
        public Screen Screen
        {
            get;
            private set;
        }

        public const string Identification = "lontium semi";

        private readonly string _name;
        string IPanel.Name => this._name;

        int IPanel.Width => this.Screen.Bounds.Width;
        //int IPanel.Width => (int)(this.Screen.Bounds.Width * (this.Screen.Bounds.Width / 7680.0)); // TODO : test here !

        int IPanel.Height => this.Screen.Bounds.Height;
        //int IPanel.Height => (int)(this.Screen.Bounds.Height * (this.Screen.Bounds.Height / 4320.0));  // TODO : test here !

        int IPanel.NumberOfColors => 256;

        int IPanel.ResponseTime => 120;

        bool IPanel.IsFullScreen => true;

        double IPanel.RatioCorrection => 1d; // TODO Adjust here !!!!

        public override string ToString()
        {
            return this._name;
        }

        public Wisecoco8k103Panel()
        {
            this._name = "External Panel";
        }

        public Wisecoco8k103Panel(Screen screen)
        {
            this._name = ScreenInterrogatory.DeviceFriendlyName(screen);
            this.Screen = screen;
        }
    }
}
