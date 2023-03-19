using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Panels
{
    internal class NoPanel : IPanel
    {
        private string _name;
        string IPanel.Name => this._name;

        int IPanel.Width => 0;

        int IPanel.Height => 0;

        int IPanel.NumberOfColors => 256;

        public override string ToString()
        {
            return this._name;
        }

        public NoPanel()
        {
            this._name = "No Panel";
        }
    }
}
