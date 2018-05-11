using MahApps.Metro.Controls;
using System;
using System.Threading;
using Thingy.API.Jobs;

namespace Thingy.InternalModules
{
    /// <summary>
    /// Interaction logic for JobRunnerWindow.xaml
    /// </summary>
    public partial class JobRunnerWindow : MetroWindow
    {
        public CancellationTokenSource TokenSource { get; }
        public Progress<JobProgress> Reporter { get; }

        public JobRunnerWindow()
        {
            InitializeComponent();
            TokenSource = new CancellationTokenSource();
            Reporter = new Progress<JobProgress>(ProgressChanged);
        }

        private void ProgressChanged(JobProgress obj)
        {
            TaskBar.ProgressValue = obj.Progress;
            ProgressBar.Value = obj.Progress;
            StatusText.Text = obj.StatusText;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TokenSource.Cancel();
        }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            TokenSource.Cancel();
        }
    }
}
