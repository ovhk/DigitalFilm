using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DigitalDarkroom.Panels
{
    internal class NoPanel : IPanel
    {
        private string _name;
        string IPanel.Name => this._name;

        private int width;

        int IPanel.Width => this.width;

        private int height;
        int IPanel.Height => this.height;

        int IPanel.NumberOfColors => 256;

        public override string ToString()
        {
            return this._name;
        }

        public NoPanel()
        {
        }

        public NoPanel(PropertyGrid pg)
        {
            this.height = pg.Height;
            this.width = pg.Width;
            this._name = "No Panel";
        }
    }
}
