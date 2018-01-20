using ManagedBass;
using ManagedBass.Wasapi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.MusicPlayerCore
{
    public sealed class AudioEngine : IAudioEngine, IDisposable
    {
        private int _deviceIndex;

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
        }

        private void InitBassDLL()
        {
            // not playing anything via BASS, so don't need an update thread
            Bass.UpdateThreads = 0;
            WasapiDeviceInfo di;
            BassWasapi.GetDeviceInfo(PlayBackDeviceIndex, out di);
            //Bass is only used for decoding, default device mix frequency is used for MOD
            Bass.Init(0, di.MixFrequency, DeviceInitFlags.Default);
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
                Bass.Free();
                _deviceIndex = value;
                InitBassDLL();
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            BassWasapi.Unload();
            Bass.PluginFree(0);
            Bass.Unload();
            GC.SuppressFinalize(this);
        }
    }
}
