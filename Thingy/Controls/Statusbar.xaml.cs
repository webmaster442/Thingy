using CoreAudioApi;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.API;
using Thingy.Implementation;
using Thingy.InternalCode;

namespace Thingy.Controls
{
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    internal partial class StatusbarView : UserControl
    {
        private static PerformanceCounter _cpuCounter;
        private static long _availableMemory;
        private MMDevice defaultDevice;
        private bool external;

        private DispatcherTimer _timer;
        public IApplication Application { get; set; }

        public StatusbarView()
        {
            InitializeComponent();

            _availableMemory = PerformanceInfo.GetTotalMemoryInMiB();
            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000),
                IsEnabled = true
            };
            _timer.Tick += _timer_Tick;
            _timer_Tick(null, null);

            SetupAudio();
        }

        private void SetupAudio()
        {
            try
            {
                MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
                defaultDevice = devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
                defaultDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                external = true;
                BtnMute.IsChecked = defaultDevice.AudioEndpointVolume.Mute;
                VolumeSlider.Value = defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0f;
                external = false;
            }
            catch (Exception e)
            {
                Application.Log.Error(e);
                VolumeSlider.IsEnabled = false;
                BtnMute.IsEnabled = false;
            }
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            Dispatcher.Invoke(() =>
            {
                external = true;
                BtnMute.IsChecked = data.Muted;
                VolumeSlider.Value = data.MasterVolume * 100.0;
                external = false;
            });
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            CPUProgress.Value = _cpuCounter.NextValue();
            CPUText.Text = string.Format("{0:0.00}%", CPUProgress.Value);

            double ram = 100.0 - ((double) PerformanceInfo.GetPhysicalAvailableMemoryInMiB() / _availableMemory) * 100.0;

            RAMProgress.Value = ram;
            RAMText.Text = string.Format("{0:0.00}%", ram);
            RAMAmount.Text = string.Format("{0} MB", PerformanceInfo.GetPhysicalAvailableMemoryInMiB());

            BatteryInfo.UpdateBatteryInfo();
        }

        private void Slider_ValueChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<double> e)
        {
            if (!external)
            {
                defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar = (float)(VolumeSlider.Value / 100.0);
            }
        }

        private void BtnMute_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            defaultDevice.AudioEndpointVolume.Mute = (bool)BtnMute.IsChecked;
        }

        private void DisplaySwitch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.ShowStatusBarMenu(new MonitorSwitcher(), "Display swithcer");
        }

        private void Power_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Application.ShowStatusBarMenu(new WindowsPower(), "Power options");
        }
    }
}
