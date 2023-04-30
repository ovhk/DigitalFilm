using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalFilm.Modes
{
    /// <summary>
    /// Interface for modes
    /// </summary>
    public interface IMode
    {
        string Name { get; }
        string Description { get; }
        bool Load();
        bool Unload();
    }
}
