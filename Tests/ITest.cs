using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDarkroom.Tests
{
    public interface ITest
    {
        string Name { get; }
        bool Load(int duration);
        bool Unload();
    }
}
