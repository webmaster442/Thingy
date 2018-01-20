using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.MusicPlayerCore
{
    public sealed class AudioEngine : IAudioEngine, IDisposable
    {
        public AudioEngine()
        {
            Log = new AudioEngineLog();
        }
        
        public AudioEngineLog Log { get; }

        public bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        public void Dispose()
        {
        }
    }
}
