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

        private string _name;
        string IPanel.Name => this._name;
        int IPanel.Width => this.Screen.Bounds.Width;

        int IPanel.Height => this.Screen.Bounds.Height;

        int IPanel.NumberOfColors => 256;

        int IPanel.ResponseTime => 120;  // TODO : 30 Hz so consider x2 = 80 ms ????

        bool IPanel.IsFullScreen => true;

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
