using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Panels
{
    public interface IPanel
    {
        string Name { get; }
        int Width { get; }
        int Height { get; }
    }
}
