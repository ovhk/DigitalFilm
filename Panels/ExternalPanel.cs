using DigitalDarkroom.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom.Panels
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

        int IPanel.ResponseTime => 120;

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
