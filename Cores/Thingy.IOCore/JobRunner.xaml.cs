using MahApps.Metro.Controls;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;

namespace Thingy.JobCore
{
    /// <summary>
    /// Interaction logic for JobRunner.xaml
    /// </summary>
    public partial class JobRunner : MetroWindow
    {
        private readonly IApplication _app;
        private readonly AsyncJob _job;
        private readonly CancellationTokenSource _tokenSource;
        private readonly Progress<JobProgress> _reporter;

        public JobRunner(IApplication app, AsyncJob job)
        {
            InitializeComponent();
            _tokenSource = new CancellationTokenSource();
            _reporter = new Progress<JobProgress>(ProgressChanged);
            _app = app;
            _job = job;
        }

        private void ProgressChanged(JobProgress obj)
        {
            TaskBar.ProgressValue = obj.Progress;
            ProgressBar.Value = obj.Progress;
            StatusText.Text = obj.StatusText;
        }

        private void PART_NegativeButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _tokenSource.Cancel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _tokenSource.Cancel();
        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _app.Log.Info("Starting job: {0}", _job.GetType().FullName);
                await _job.Run(_tokenSource.Token, _reporter);
                _app.Log.Info("Finished job: {0}", _job.GetType().FullName);
                Close();
            }
            catch (TaskCanceledException)
            {
                _app.Log.Info("{0} Job canceled by user", _job.GetType().FullName);
                Close();
            }
            catch (Exception ex)
            {
                _app.Log.Error("Exception in job: {0}", _job.GetType().FullName);
                _app.Log.Error(ex);
                Close();
            }
        }
    }
}
