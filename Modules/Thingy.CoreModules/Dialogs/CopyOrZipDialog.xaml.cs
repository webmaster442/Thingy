using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Thingy.API;

namespace Thingy.CoreModules.Dialogs
{
    public partial class CopyorZipDialog : Window
    {
        private CancellationTokenSource _cancellationTokenSource;
        private bool _taskrunning;
        private Progress<double> _progress;
        private IApplication _app;

        public CopyorZipDialog(IApplication app)
        {
            InitializeComponent();
            _progress = new Progress<double>(DisplayProgress);
            _app = app;
        }

        private void DisplayProgress(double obj)
        {
            ProgressIndicator.Value = obj;
        }

        private void ReportProgress(IProgress<double> progress, long totaldone, long totalsize)
        {
            if (progress != null)
            {
                double percent = (double)totaldone / (double)totalsize;
                percent = Math.Round(percent * 100, 2);
                progress.Report(percent);
            }
        }

        private bool CopyTaskJob(IList<string> files,
                                 string destination,
                                 IProgress<double> progress,
                                 CancellationToken ct)
        {
            long totalsize = 0;
            long totaldone = 0;
            byte[] buffer = new byte[4096 * 2];
            int bufferFill;
            List<string> destinations = new List<string>(files.Count);

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                totalsize += fi.Length;
                destinations.Add(Path.Combine(destination, fi.Name));
            }

            try
            {
                for (int i = 0; i < files.Count; i++)
                {
                    using (var source = File.OpenRead(files[i]))
                    {
                        using (var target = File.Create(destinations[i]))
                        {
                            do
                            {
                                bufferFill = source.Read(buffer, 0, buffer.Length);
                                ct.ThrowIfCancellationRequested();
                                totaldone += bufferFill;
                                target.Write(buffer, 0, bufferFill);
                                ReportProgress(progress, totaldone, totalsize);
                                //await Task.Delay(100);
                            }
                            while (bufferFill > 0);
                        }
                    }
                }
            }
            catch (OperationCanceledException ocex)
            {
                _app.Log.Error(ocex);
                return true;
            }
            catch (Exception ex)
            {
                _app.Log.Error(ex);
                return false;
            }

            return true;
        }

        private bool ZipTaskJob(IList<string> files,
                                string destination,
                                IProgress<double> progress,
                                CancellationToken ct)
        {
            long totalsize = 0;
            long totaldone = 0;
            byte[] buffer = new byte[4096 * 2];
            int bufferFill;
            List<string> destinations = new List<string>(files.Count);

            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                totalsize += fi.Length;
                destinations.Add(fi.Name);
            }

            try
            {
                using (var target = File.Create(destination))
                {
                    using (var zip = new ZipArchive(target, ZipArchiveMode.Create, true))
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            using (var source = File.OpenRead(files[i]))
                            {
                                var entry = zip.CreateEntry(destinations[i]);
                                using (var packed = entry.Open())
                                {
                                    do
                                    {
                                        bufferFill = source.Read(buffer, 0, buffer.Length);
                                        ct.ThrowIfCancellationRequested();
                                        totaldone += bufferFill;
                                        packed.Write(buffer, 0, bufferFill);
                                        ReportProgress(progress, totaldone, totalsize);
                                        //await Task.Delay(100);
                                    }
                                    while (bufferFill > 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (OperationCanceledException ocex)
            {
                _app.Log.Error(ocex);
                return true;
            }
            catch (Exception ex)
            {
                _app.Log.Error(ex);
                return false;
            }

            return true;
        }

        private Task<bool> ZipTask(IList<string> files,
                                   string destination,
                                   IProgress<double> progress,
                                   CancellationToken ct)
        {
            return Task.Run<bool>(() =>
            {
                return ZipTaskJob(files, destination, progress, ct);
            });
        }

        private Task<bool> CopyTask(IList<string> files,
                                    string destination,
                                    IProgress<double> progress,
                                    CancellationToken ct)
        {
            return Task.Run<bool>(() =>
            {
                return CopyTaskJob(files, destination, progress, ct);
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_taskrunning)
            {
                var q = MessageBox.Show("Cancel running task?", "Task running", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (q == MessageBoxResult.OK)
                {
                    if (_taskrunning) _cancellationTokenSource.Cancel();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource.Cancel();
        }


        public async void StartCopy(IList<string> files, string destination)
        {
            try
            {
                Title = "Copy";
                DialogText.Text = "Copy in progress...";
                _cancellationTokenSource = new CancellationTokenSource();
                _taskrunning = true;
                bool result = await CopyTask(files, destination, _progress, _cancellationTokenSource.Token);
                _taskrunning = false;
                if (!result)
                {
                    MessageBox.Show("Error on copy", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                _app.Log.Error(ex);
                _taskrunning = false;
                Close();
            }
        }

        public async void StartZip(IList<string> files, string destination)
        {
            try
            {
                Title = "Packing";
                DialogText.Text = "Packing in progress...";
                _cancellationTokenSource = new CancellationTokenSource();
                _taskrunning = true;
                bool result = await ZipTask(files, destination, _progress, _cancellationTokenSource.Token);
                _taskrunning = false;
                if (!result)
                {
                    MessageBox.Show("Error on packing", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception ex)
            {
                _app.Log.Error(ex);
                _taskrunning = false;
                Close();
            }
        }
    }
}
