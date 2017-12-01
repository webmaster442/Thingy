using CoreAudioApi;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;
using Thingy.Implementation;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for Statusbar.xaml
    /// </summary>
    public partial class Statusbar : UserControl
    {
        private static PerformanceCounter _cpuCounter;
        private static long _availableMemory;
        private MMDevice defaultDevice;
        private bool external;

        private DispatcherTimer _timer;

        public Statusbar()
        {
            InitializeComponent();

            MMDeviceEnumerator devEnum = new MMDeviceEnumerator();
            defaultDevice =  devEnum.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia);
            defaultDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;

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

            external = true;
            BtnMute.IsChecked = defaultDevice.AudioEndpointVolume.Mute;
            VolumeSlider.Value = defaultDevice.AudioEndpointVolume.MasterVolumeLevelScalar * 100.0f;
            external = false;
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
        }

        private void ScreenClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is MenuItem caller)
            {
                Process p = new Process();
                p.StartInfo.FileName = "DisplaySwitch.exe";
                p.StartInfo.Arguments = caller.Tag.ToString();
                p.Start();
            }
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
    }
}
