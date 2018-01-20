using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.MusicPlayerCore
{
    public interface IAudioEngine
    {
        AudioEngineLog Log { get; }
        bool Is64BitProcess { get; }
    }
}
