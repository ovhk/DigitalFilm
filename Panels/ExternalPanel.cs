using DigitalFilm.Tools;
using System.Windows.Forms;

namespace DigitalFilm.Panels
{
    public class ExternalPanel : IPanel
    {
        public Screen Screen
        {
            get;
            private set;
        }

        private readonly string _name;
        string IPanel.Name => this._name;
        int IPanel.Width => this.Screen.Bounds.Width;

        int IPanel.Height => this.Screen.Bounds.Height;

        int IPanel.NumberOfColors => 256;

        int IPanel.ResponseTime => 80;

        bool IPanel.IsFullScreen => true;

        double IPanel.RatioCorrection => 1d;

        public override string ToString()
        {
            return this._name;
        }

        public ExternalPanel()
        {
            this._name = "External Panel";
        }

        public ExternalPanel(Screen screen)
        {
            this._name = ScreenInterrogatory.DeviceFriendlyName(screen);
            //this._name = screen.DeviceName.Replace("\\\\.\\", "");
            this.Screen = screen;
        }
    }
}
