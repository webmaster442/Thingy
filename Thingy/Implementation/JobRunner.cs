using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shell;
using Thingy.API;
using Thingy.API.Jobs;
using Thingy.InternalModules;

namespace Thingy.Implementation
{
    public class JobRunner : IJobRunner
    {
        private readonly IApplication _app;

        public JobRunner(IApplication app)
        {
            _app = app;
        }

        private async Task ExceptionHandler(Type job, JobRunnerWindow win, Exception ex)
        {
            _app.Log.Error("Exception in job: {0}", job.FullName);
            _app.Log.Exception(ex);
            MessageBox.Show("Job terminated with an undhandled exception. See log for details");
            await Task.Delay(100);
            win.Close();
        }

        private void Canceled(Type job, JobRunnerWindow win)
        {
            _app.Log.Info("{0} Job canceled by user", job.FullName);
            win.Close();
        }

        public async Task RunJob(Job job)
        {
            JobRunnerWindow win = new JobRunnerWindow();
            win.TaskBar.ProgressState = TaskbarItemProgressState.Normal;
            try
            {
                win.Show();
                _app.Log.Info("Starting job: {0}", job.GetType().FullName);
                await job.RunJob(win.TokenSource.Token, win.Reporter);
                _app.Log.Info("Finished job: {0}", job.GetType().FullName);
                win.Close();
            }
            catch (OperationCanceledException)
            {
                Canceled(job.GetType(), win);
            }
            catch (Exception ex)
            {
                await ExceptionHandler(job.GetType(), win, ex);
            }
        }

        public async Task<T> RunJob<T>(Job<T> job) where T : class, new()
        {
            JobRunnerWindow win = new JobRunnerWindow();
            win.TaskBar.ProgressState = TaskbarItemProgressState.Normal;
            try
            {
                win.Show();
                _app.Log.Info("Starting job: {0}", job.GetType().FullName);
                await job.RunJob(win.TokenSource.Token, win.Reporter);
                _app.Log.Info("Finished job: {0}", job.GetType().FullName);
                win.Close();
                return job.Result;
            }
            catch (OperationCanceledException)
            {
                Canceled(job.GetType(), win);
                return null;
            }
            catch (Exception ex)
            {
                await ExceptionHandler(job.GetType(), win, ex);
                return null;
            }
        }
    }
}
