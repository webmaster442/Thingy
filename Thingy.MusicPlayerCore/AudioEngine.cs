using ManagedBass;
using ManagedBass.Mix;
using ManagedBass.Wasapi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore
{
    public sealed class AudioEngine : IAudioEngine, IDisposable
    {
        private int _deviceIndex;
        private int _decodeChannel;
        private int _mixerChannel;
        private float _LastVolume;
        private TagInformation _currentTags;
        private List<Chapter> _chapters;
        private double _length;

        private DispatcherTimer _updateTimer;
        private WasapiProcedure _wasapiProcess;

        public event PropertyChangedEventHandler PropertyChanged;

        public AudioEngineLog Log { get; }

        public string NativeLibPath { get; private set; }

        public bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        public AudioEngine()
        {
            Log = new AudioEngineLog();
            Log.Info("Audio Engine Load started");
            SetNativeLibPath();
            LoadLibs();
            Log.Info("Setting output to Default Device...");
            PlayBackDeviceIndex = BassWasapi.DefaultDevice;
            Log.Info("Setting up WASAPI process...");
            _wasapiProcess = WasapiProcessFunction;
            _LastVolume = 1.0f;
            _updateTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.20),
                IsEnabled = false
            };
            _updateTimer.Tick += _updateTimer_Tick;
        }

        private void _updateTimer_Tick(object sender, EventArgs e)
        {
            if (Position > Length - 0.05)
            {
                
            }
            NotifyChanged(nameof(Position));
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
            Log.Info("Loading basswasapi.dll...");
            BassWasapi.Load(NativeLibPath);
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

        private void NotifyChanged([CallerMemberName]string property = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void InitBassDLL()
        {
            // not playing anything via BASS, so don't need an update thread
            Bass.UpdateThreads = 0;
            WasapiDeviceInfo di;
            BassWasapi.GetDeviceInfo(PlayBackDeviceIndex, out di);
            //Bass is only used for decoding, default device mix frequency is used for MOD
            if (!Bass.Init(0, di.MixFrequency, DeviceInitFlags.Default))
            {   
                Log.Error(Bass.LastError.ToString());
            }
        }

        /// <inheritdoc />
        public IDictionary<string, int> OutputDevices
        {
            get
            {
                Log.Info("Querying WASAPI output devices....");
                Dictionary<string, int> devices = new Dictionary<string, int>();
                for (int i=0; i<BassWasapi.DeviceCount; i++)
                {
                    WasapiDeviceInfo info;
                    BassWasapi.GetDeviceInfo(i, out info);
                    if (info.IsEnabled && !info.IsInput)
                    {
                        devices.Add(info.Name, i);
                    }
                }
                Log.Info("Total number of WASAPI output devices: {0}", devices.Count);
                return devices;
            }
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
                Bass.Free();
                _deviceIndex = value;
                InitBassDLL();
            }
        }

        /// <inheritdoc />
        public double Position
        {
            get
            {
                int delay = BassWasapi.GetData(null, 0);
                var pos = BassMix.ChannelGetPosition(_decodeChannel, PositionFlags.Bytes, delay);
                return Bass.ChannelBytes2Seconds(_decodeChannel, pos);
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

        /// <inheritdoc />
        public void Load(string fileName)
        {
            _decodeChannel = Bass.CreateStream(fileName, 0, 0, BassFlags.Decode | BassFlags.Float | BassFlags.AutoFree);
            var channelInfo = Bass.ChannelGetInfo(_decodeChannel);
            //BASS_WASAPI_Init(device,ci.freq,ci.chans,flags,buflen,0.05,WasapiProc,NULL)

            WasapiInitFlags initFlags = WasapiInitFlags.Shared | WasapiInitFlags.EventDriven;

            bool wasapi_init = BassWasapi.Init(PlayBackDeviceIndex,
                                               channelInfo.Frequency,
                                               channelInfo.Channels,
                                               initFlags, 0.05f, 0.05f, _wasapiProcess);
            if (!wasapi_init)
            {
                Log.Error(Bass.LastError.ToString());
                return;
            }
            WasapiInfo wasapi_info;
            if (!BassWasapi.GetInfo(out wasapi_info))
            {
                Log.Error(Bass.LastError.ToString());
                return;
            }
            Log.Info("Initialized Device {0} Hz {0} Ch output", wasapi_info.Frequency, wasapi_info.Channels);

            BassFlags mixerflags = BassFlags.Float | BassFlags.MixerPositionEx | BassFlags.AutoFree;
            _mixerChannel = BassMix.CreateMixerStream(wasapi_info.Frequency, wasapi_info.Channels, mixerflags);
            BassMix.MixerAddChannel(_mixerChannel, _decodeChannel, BassFlags.MixerDownMix);
            if (!BassWasapi.Start())
            {
                Log.Error(Bass.LastError.ToString());
                return;
            }
            BassWasapi.Lock(true);

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

            Bass.ChannelSetAttribute(_mixerChannel, ChannelAttribute.Volume, _LastVolume);
            Log.Info("Loaded file {0}", fileName);
        }

        private int WasapiProcessFunction(IntPtr Buffer, int Length, IntPtr User)
        {
            return Bass.ChannelGetData(_mixerChannel, Buffer, Length);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            BassWasapi.Lock(false);
            if (_mixerChannel != 0)
            {
                Bass.StreamFree(_mixerChannel);
            }
            if (_decodeChannel != 0)
            {
                Bass.StreamFree(_decodeChannel);
            }
            BassMix.Unload();
            BassWasapi.Unload();
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
        }
    }
}
