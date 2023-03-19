using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Modes
{
    public interface IMode
    {
        string Name { get; }
        string Description { get; }
        bool Load(string[] imgPaths, int duration);
        bool Unload();
    }
}
