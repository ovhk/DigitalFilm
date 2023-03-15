using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Panels
{
    public class WisecocoTOP103MONO8K01A : IPanel
    {
        private string _name;
        string IPanel.Name => this._name;

        int IPanel.Width => 7680;

        int IPanel.Height => 4320;

        public override string ToString()
        {
            return this._name;
        }

        public WisecocoTOP103MONO8K01A()
        {
            this._name = "TOP103MONO8K01A";
        }
    }
}
