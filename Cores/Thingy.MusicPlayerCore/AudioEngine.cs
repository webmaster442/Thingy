using AppLib.Common.Extensions;
using ManagedBass;
using ManagedBass.Cd;
using ManagedBass.Mix;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Thingy.MusicPlayerCore.DataObjects;
using Thingy.MusicPlayerCore.Formats;
using Thingy.Resources;

namespace Thingy.MusicPlayerCore
{
    public sealed class AudioEngine : IAudioEngine
    {
        private int _deviceIndex;
        private int _decodeChannel;
        private int _mixerChannel;
        private float _LastVolume;
        private TagInformation _currentTags;
        private double _length;
        private DownloadProcedure _streamDloadProc;
        private bool _networkstream;
        private string _netadress;

        private static readonly object Lock = new object();
        private int _req;

        private DispatcherTimer _updateTimer;
        private double _progressPercent;

        public event PropertyChangedEventHandler PropertyChanged;
        public event RoutedEventHandler SongFinishedEvent;

        public AudioEngineLog Log { get; }

        /// <inheritdoc />
        public string NativeLibPath { get; private set; }

        /// <inheritdoc />
        public bool Is64BitProcess
        {
            get { return IntPtr.Size == 8; }
        }

        /// <inheritdoc />
        public IExtensionProvider ExtensionProvider
        {
            get;
            private set;
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
            _streamDloadProc = new DownloadProcedure(StreamDownloadProcedure);
            Chapters = new ObservableCollection<Chapter>();
            ExtensionProvider = new ExtensionProvider();
        }

        private void Reset()
        {
            Position = 0;
            _currentTags = null;
            Chapters?.Clear();
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
                if (Position > Length - 0.05 && !_networkstream)
                {
                    SongFinishedEvent?.Invoke(this, new RoutedEventArgs());
                }
                _progressPercent = (Position / Length) * 100;
                NotifyChanged(nameof(Position));
                NotifyChanged(nameof(ProgessPercent));
            }
        }

        /// <inheritdoc />
        private void SetNativeLibPath()
        {
            var enginedir = AppDomain.CurrentDomain.BaseDirectory;
            if (Is64BitProcess)
                NativeLibPath = Path.Combine(enginedir, @"Native\x64");
            else
                throw new Exception("x64 operating system expected!");

            Log.Info("Native Libary location: {0}", NativeLibPath);
        }

        /// <inheritdoc />
        private void LoadLibs()
        {
            Log.Info("Loading bass.dll...");
            Bass.Load(NativeLibPath);
            Log.Info("Loading bassmix.dll...");
            BassMix.Load(NativeLibPath);
            Log.Info("Loading basscd.dll...");
            BassCd.Load(NativeLibPath);
            LoadPlugins("bass_aac.dll", "bass_ac3.dll",
                        "bassalac.dll", "bassflac.dll", 
                        "basswma.dll", "basswv.dll");
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
                        devices.AddOrUpdate(device.Name, i);
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

        public ObservableCollection<Chapter> Chapters
        {
            get;
            private set;
        }

        public bool Seeking { get; set; }

        public double ProgessPercent
        {
            get { return _progressPercent; }
        }

        /// <inheritdoc />
        public void Load(string fileName)
        {
            _networkstream = ExtensionProvider.IsNetworkStream(fileName);

            Log.Info("Loading file: {0}", fileName);

            if (_decodeChannel != 0 || _mixerChannel != 0)
            {
                Stop();
            }

            try { Bass.StreamFree(_decodeChannel); }
            catch (Exception ex) { Debug.WriteLine(ex); }

            if (string.IsNullOrEmpty(fileName))
            {
                Log.Error("Filename was null or empty. Aborted load");
                return;
            }

            var sourceflags = BassFlags.Decode | BassFlags.Loop | BassFlags.Float | BassFlags.Prescan;
            var mixerflags = BassFlags.MixerDownMix | BassFlags.MixerPositionEx | BassFlags.AutoFree;

            if (_networkstream)
            {
                int r;
                lock (Lock)
                {
                    // make sure only 1 thread at a time can do the following
                    // increment the request counter for this request
                    r = ++_req;
                }
                var netFlags = BassFlags.StreamDownloadBlocks | sourceflags;
                Bass.NetProxy = ""; //os default proxy
                _decodeChannel = Bass.CreateStream(fileName, 0, netFlags, _streamDloadProc, new IntPtr(r));
                lock (Lock)
                {
                    if (r != _req)
                    {
                        if (_decodeChannel != 0) Bass.StreamFree(_decodeChannel);
                        return;
                    }
                }
                _netadress = fileName;
            }
            else
            {
                if (ExtensionProvider.IsCdStream(fileName))
                {
                    var cd = new CDTrackInfo(fileName);
                    _decodeChannel = BassCd.CreateStream(cd.Drive, cd.Track, sourceflags);
                    Log.Info("Geting track metadata...");
                    UpdateCDTags(cd.Drive, cd.Track);
                }
                else
                {
                    _decodeChannel = Bass.CreateStream(fileName, 0, 0, sourceflags);
                    Log.Info("Geting track metadata...");
                    UpdateFileTags(fileName);
                }
            }

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

            if (!_networkstream)
            {
                Log.Info("Getting track length...");
                var len = Bass.ChannelGetLength(_decodeChannel, PositionFlags.Bytes);
                _length = Bass.ChannelBytes2Seconds(_decodeChannel, len);
                NotifyChanged(nameof(Length));

                Log.Info("Getting Chapters...");
                Chapters.Clear();
                Chapters.AddRange(ChapterFactory.GetChapters(fileName, _length));
            }

            Volume = _LastVolume;
            Bass.ChannelSetAttribute(_mixerChannel, ChannelAttribute.Volume, Volume);
            Log.Info("Loaded file {0}", fileName);
        }

        private async void UpdateFileTags(string fileName)
        {
            _currentTags = await TagFactory.CreateTagInfoFromFile(fileName);
            NotifyChanged(nameof(CurrentTags));
        }

        private async void UpdateCDTags(int drive, int track)
        {
            _currentTags = await TagFactory.CreateTagInfoForCD(drive, track);
            NotifyChanged(nameof(CurrentTags));
        }

        private void StreamDownloadProcedure(IntPtr Buffer, int Length, IntPtr User)
        {
            var ptr = Bass.ChannelGetTags(_decodeChannel, TagType.META);
            if (ptr != IntPtr.Zero)
            {
                var array = Extensions.ExtractMultiStringUtf8(ptr);
                if (array != null)
                    ProcessTags(array);
            }
            else
            {
                ptr = Bass.ChannelGetTags(_decodeChannel, TagType.OGG);
                if (ptr != null)
                {
                    var array = Extensions.ExtractMultiStringUtf8(ptr); //Marshal.PtrToStringAnsi(ptr);
                    if (array != null)
                        ProcessTags(array, true);
                }
            }
        }

        private async Task UpdateTags(TagInformation tags)
        {
            _currentTags = tags;
            var bytes = await iTunesCoverDownloader.GetCoverFor($"{tags.Title}");
            if (bytes != null)
                _currentTags.Cover = iTunesCoverDownloader.CreateBitmap(bytes); 
            else
                _currentTags.Cover = new BitmapImage(ResourceLocator.GetIcon(IconCategories.Big, "icons8-radio-540.png"));
            NotifyChanged(nameof(CurrentTags));
        }

        private void ProcessTags(string[] array, bool icecast = false)
        {
            string artist = null;
            string title = null;
            if (icecast)
            {
                foreach (var item in array)
                {
                    if (item.StartsWith("ARTIST")) artist = item.Replace("ARTIST=", "");
                    else if (item.StartsWith("TITLE")) title = item.Replace("TITLE=", " - ");
                    else continue;
                }
                var newtags = TagFactory.CreateTagInfoForNetStream(_netadress, title, artist);
                if (newtags != _currentTags && !string.IsNullOrEmpty(artist))
                {
                    Application.Current.Dispatcher.Invoke(async() =>
                    {
                        await UpdateTags(newtags);
                    });
                }
            }
            else
            {
                var contents = array[0].Split(';');
                foreach (var item in contents)
                {
                    if (item.StartsWith("StreamTitle='")) title = item.Replace("StreamTitle='", "").Replace("'", "");
                    else continue;
                }
                var newtags = TagFactory.CreateTagInfoForNetStream(_netadress, title, artist);
                if (newtags != _currentTags && !string.IsNullOrEmpty(title))
                {
                    Application.Current.Dispatcher.Invoke(async() =>
                    {
                        await UpdateTags(newtags);
                    });
                }
            }
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
            Bass.ChannelPause(_mixerChannel);
            Bass.StreamFree(_mixerChannel);
            Reset();
        }
    }
}
