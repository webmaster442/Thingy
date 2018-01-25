using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore
{
    public interface IAudioEngine: INotifyPropertyChanged, IDisposable
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
        /// Gets the current stream's length in seconds
        /// </summary>
        double Length { get; }
        /// <summary>
        /// Get or set channel volume
        /// </summary>
        float Volume { get; set; }
        /// <summary>
        /// Play
        /// </summary>
        void Play();
        /// <summary>
        /// Pause playback
        /// </summary>
        void Pause();
        /// <summary>
        /// Stop playback
        /// </summary>
        void Stop();
        /// <summary>
        /// Currently played song's metadata
        /// </summary>
        TagInformation CurrentTags { get; }
        /// <summary>
        /// Chapter list for currently played song
        /// </summary>
        IList<Chapter> Chapters { get; }
        /// <summary>
        /// Event, that gets fired when the current song is finished
        /// </summary>
        event RoutedEventHandler SongFinishedEvent;
        /// <summary>
        /// Set to true, when you want to change position in song
        /// </summary>
        bool Seeking { get; set; }
    }
}
