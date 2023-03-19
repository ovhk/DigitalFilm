using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DigitalDarkroom.Panels
{
    public class PanelSimulator : IPanel
    {
        private string _name;
        string IPanel.Name => this._name;

        int IPanel.Width => 1024;

        int IPanel.Height => 768;

        int IPanel.NumberOfColors => 256;

        public override string ToString()
        {
            return this._name;
        }

        public PanelSimulator()
        {
            this._name = "Simulator";
        }
    }
}
