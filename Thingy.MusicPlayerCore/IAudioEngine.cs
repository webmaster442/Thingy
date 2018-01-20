using System.Collections.Generic;

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
        /// <summary>
        /// Get or set stream position in seconds
        /// </summary>
        double Position { get; set; }
        /// <summary>
        /// Get or set channel volume
        /// </summary>
        float Volume { get; set; }
        /// <summary>
        /// Play
        /// </summary>
        void Play();
        /// <summary>
        /// Pause
        /// </summary>
        void Pause();
        /// <summary>
        /// Stop
        /// </summary>
        void Stop();
    }
}
