using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.MusicPlayerCore
{
    public interface IAudioEngine
    {
        /// <summary>
        /// Engine Log
        /// </summary>
        AudioEngineLog Log { get; }
        /// <summary>
        /// Path of native dll files
        /// </summary>
        string NativeLibPath { get; }
        /// <summary>
        /// True, if the hosting process is a 64 bit app
        /// </summary>
        bool Is64BitProcess { get; }
        /// <summary>
        /// List of output devices and their indexes
        /// </summary>
        IDictionary<string, int> OutputDevices { get; }
        /// <summary>
        /// Gets or sets the Currently used playback device
        /// </summary>
        int PlayBackDeviceIndex { get; set; }
        /// <summary>
        /// Load a file for playback
        /// </summary>
        /// <param name="file">File path</param>
        void Load(string file);
    }
}
