using DigitalFilm.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalFilm.Panels
{
    internal class Wisecoco8k103 : IPanel
    {
        public Screen Screen
        {
            get;
            private set;
        }

        private string _name;
        string IPanel.Name => this._name;
        int IPanel.Width => this.Screen.Bounds.Width;

        int IPanel.Height => this.Screen.Bounds.Height;

        int IPanel.NumberOfColors => 256;

        int IPanel.ResponseTime => 120;

        bool IPanel.IsFullScreen => true;

        double IPanel.RatioCorrection => 1d; // TODO Adjust here !!!!

        public override string ToString()
        {
            return this._name;
        }

        public Wisecoco8k103()
        {
            this._name = "External Panel";
        }

        public Wisecoco8k103(Screen screen)
        {
            this._name = ScreenInterrogatory.DeviceFriendlyName(screen);
            this.Screen = screen;
        }
    }
}
