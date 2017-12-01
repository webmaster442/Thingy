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

        private DispatcherTimer _timer;

        public Statusbar()
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
    }
}
