using ManagedBass;
using ManagedBass.Mix;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore
{
    public sealed class AudioEngine : IAudioEngine
    {
        private int _deviceIndex;
        private int _decodeChannel;
        private int _mixerChannel;
        private float _LastVolume;
        private TagInformation _currentTags;
        private List<Chapter> _chapters;
        private double _length;

        private DispatcherTimer _updateTimer;

        public event PropertyChangedEventHandler PropertyChanged;
        public event RoutedEventHandler SongFinishedEvent;

        public AudioEngineLog Log { get; }

        public string NativeLibPath { get; private set; }

        public bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        public AudioEngine()
        {
            DefaultDeviceIndex = -1;
            Log = new AudioEngineLog();
            Log.Info("Audio Engine Load started");
            SetNativeLibPath();
            LoadLibs();
            _LastVolume = 1.0f;
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(40),
                IsEnabled = false
            };
            _updateTimer.Tick += TimerTick;
            Log.Info("Setting output to Default Device...");
            PlayBackDeviceIndex = -1;
            _chapters = new List<Chapter>();
        }

        private void Reset()
        {
            Position = 0;
            _currentTags = null;
            _chapters?.Clear();
            _length = 0;
            NotifyChanged(nameof(Position));
            NotifyChanged(nameof(CurrentTags));
            NotifyChanged(nameof(Chapters));
            NotifyChanged(nameof(Length));
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (!Seeking)
            {
                if (Position > Length - 0.05)
                {
                    SongFinishedEvent?.Invoke(this, new RoutedEventArgs());
                }
                NotifyChanged(nameof(Position));
            }
        }

        /// <inheritdoc />
        private void SetNativeLibPath()
        {
            var enginedir = AppDomain.CurrentDomain.BaseDirectory;
            if (Is64BitProcess)
                NativeLibPath = Path.Combine(enginedir, @"NativeLibs\x64");
            else
                NativeLibPath = Path.Combine(enginedir, @"NativeLibs\x86");

            Log.Info("Native Libary location: {0}", NativeLibPath);
        }

        /// <inheritdoc />
        private void LoadLibs()
        {
            Log.Info("Loading bass.dll...");
            Bass.Load(NativeLibPath);
            Log.Info("Loading bassmix.dll...");
            BassMix.Load(NativeLibPath);
            LoadPlugins("bass_aac.dll", "bass_ac3.dll",
                        "bassalac.dll", "basscd.dll",
                        "bassflac.dll", "basswma.dll",
                        "basswv.dll");
        }

        private void LoadPlugins(params string[] plugins)
        {
            foreach (var plugin in plugins)
            {
                Log.Info("Loading Plugin {0}...", plugin);
                var fullpath = Path.Combine(NativeLibPath, plugin);
                Bass.PluginLoad(fullpath);
            }
        }

        private void NotifyChanged(params string[] properties)
        {
            if (PropertyChanged != null)
            {
                foreach (var property in properties)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
                }
            }
        }

        private void InitDevice()
        {
            if (!Bass.Init(_deviceIndex, 48000, DeviceInitFlags.Frequency))
            {
                Log.Error("Device init failed: {0}", _deviceIndex);
            }
            else
            {
                Log.Info("Device init ok: {0}", _deviceIndex);
                Bass.Start();
            }

        }

        private void NotifyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <inheritdoc />
        public IDictionary<string, int> OutputDevices
        {
            get
            {
                var devices = new Dictionary<string, int>();
                for (int i = 1; i < Bass.DeviceCount; i++)
                {
                    var device = Bass.GetDeviceInfo(i);
                    if (device.IsEnabled)
                    {
                        devices.Add(device.Name, i);
                        if (device.IsDefault)
                        {
                            if (DefaultDeviceIndex == -1)
                            {
                                DefaultDeviceIndex = i;
                                NotifyChanged(nameof(DefaultDeviceIndex));
                            }
                        }
                    }
                }
                return devices;
            }
        }

        /// <inheritdoc />
        public int DefaultDeviceIndex
        {
            get;
            set;
        }

        /// <inheritdoc />
        public int PlayBackDeviceIndex
        {
            get { return _deviceIndex; }
            set
            {
                Stop();
                if (_decodeChannel != 0)
                    Bass.StreamFree(_decodeChannel);
                if (_mixerChannel != 0)
                    Bass.StreamFree(_mixerChannel);
                _deviceIndex = value;
                Bass.Free();
                InitDevice();
                NotifyChanged(nameof(PlayBackDeviceIndex));
            }
        }

        /// <inheritdoc />
        public double Position
        {
            get
            {
                var pos = BassMix.ChannelGetPosition(_decodeChannel, PositionFlags.Bytes);
                double seconds = Bass.ChannelBytes2Seconds(_decodeChannel, pos);
                if (seconds < 0) return 0;
                else return seconds;
            }
            set
            {
                var pos = Bass.ChannelSeconds2Bytes(_decodeChannel, value);
                Bass.ChannelSetPosition(_decodeChannel, pos);
            }
        }

        /// <inheritdoc />
        public float Volume
        {
            get
            {
                float temp = 0.0f;
                Bass.ChannelGetAttribute(_mixerChannel, ChannelAttribute.Volume, out temp);
                return temp;
            }
            set
            {
                Bass.ChannelSetAttribute(_mixerChannel, ChannelAttribute.Volume, value);
                _LastVolume = value;
                NotifyChanged();
            }
        }

        public TagInformation CurrentTags
        {
            get { return _currentTags; }
        }

        public double Length
        {
            get { return _length; }
        }

        public IList<Chapter> Chapters
        {
            get { return _chapters; }
        }

        public bool Seeking { get; set; }

        /// <inheritdoc />
        public void Load(string fileName)
        {
            Log.Info("Loading file: {0}", fileName);

            if (_decodeChannel != 0)
            {
                Stop();
                Bass.StreamFree(_decodeChannel);
                _decodeChannel = 0;
            }
            if (_mixerChannel != 0)
            {
                Bass.StreamFree(_mixerChannel);
                _mixerChannel = 0;
            }

            Reset();

            if (string.IsNullOrEmpty(fileName))
            {
                Log.Error("Filename was null or empty. Aborted load");
                return;
            }

            var sourceflags = BassFlags.Decode | BassFlags.Loop | BassFlags.Float | BassFlags.Prescan;
            var mixerflags = BassFlags.MixerDownMix | BassFlags.MixerPositionEx | BassFlags.AutoFree;

            _decodeChannel = Bass.CreateStream(fileName, 0, 0, sourceflags);
            if (_decodeChannel == 0)
            {
                Log.Error("Decode chanel creation failed: {0}", Bass.LastError);
                return;
            }

            var channelInfo = Bass.ChannelGetInfo(_decodeChannel);
            _mixerChannel = BassMix.CreateMixerStream(channelInfo.Frequency, channelInfo.Channels, mixerflags);
            if (_mixerChannel == 0)
            {
                Log.Error("Mixer chanel creation failed: {0}", Bass.LastError);
                return;
            }
            if (!BassMix.MixerAddChannel(_mixerChannel, _decodeChannel, BassFlags.MixerDownMix))
            {
                Log.Error("Failed to route decoded stream to mixer: {0}", Bass.LastError);
                return;
            }

            Log.Info("Geting track metadata...");
            _currentTags = TagFactory.CreateTagInfoFromFile(fileName);
            NotifyChanged(nameof(CurrentTags));

            Log.Info("Getting track length...");
            var len = Bass.ChannelGetLength(_decodeChannel, PositionFlags.Bytes);
            _length = Bass.ChannelBytes2Seconds(_decodeChannel, len);
            NotifyChanged(nameof(Length));

            Log.Info("Getting Chapters...");
            _chapters.Clear();
            _chapters.AddRange(ChapterFactory.GetChapters(fileName, _length));
            NotifyChanged(nameof(Chapters));

            Volume = _LastVolume;
            Bass.ChannelSetAttribute(_mixerChannel, ChannelAttribute.Volume, Volume);
            Log.Info("Loaded file {0}", fileName);
        }

        private int WasapiProcessFunction(IntPtr Buffer, int Length, IntPtr User)
        {
            return Bass.ChannelGetData(_mixerChannel, Buffer, Length);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Stop();
            if (_mixerChannel != 0)
            {
                Bass.StreamFree(_mixerChannel);
            }
            if (_decodeChannel != 0)
            {
                Bass.StreamFree(_decodeChannel);
            }
            BassMix.Unload();
            Bass.PluginFree(0);
            Bass.Unload();
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Play()
        {
            _updateTimer.Start();
            Bass.ChannelPlay(_mixerChannel, false);
        }

        /// <inheritdoc />
        public void Pause()
        {
            _updateTimer.Stop();
            Bass.ChannelPause(_mixerChannel);
        }

        /// <inheritdoc />
        public void Stop()
        {
            _updateTimer.Stop();
            Bass.ChannelStop(_mixerChannel);
            Reset();
        }
    }
}
