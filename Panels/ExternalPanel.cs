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
        private Screen _screen;

        public Screen Screen => this._screen;

        private string _name;
        string IPanel.Name => this._name;
        int IPanel.Width => this._screen.Bounds.Width;

        int IPanel.Height => this._screen.Bounds.Height;

        int IPanel.NumberOfColors => 256;

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
            this._name = screen.DeviceName;
            this._screen = screen;
        }
    }
}
