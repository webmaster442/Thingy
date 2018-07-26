using AppLib.MVVM;
using Thingy.API;
using Thingy.Implementation.Bang;
using Thingy.InternalModules;
using System.Diagnostics;
using System;
using System.Threading.Tasks;

namespace Thingy.InternalViewModels
{
    internal class BangViewModel: ViewModel<IBangView>
    {
        private readonly IApplication _app;
        private string _BangSearchText;

        public BangViewModel(IBangView view, IApplication app): base(view)
        {
            _app = app;
            BangProvider = new BangProvider();
            CancelCommand = Command.CreateCommand(Cancel);
            OkCommand = Command.CreateCommand(Ok);
            SearchCommand = Command.CreateCommand(Search);
            OpenHelpCommand = Command.CreateCommand(OpenHelp);
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand OkCommand { get; }
        public DelegateCommand SearchCommand { get; }
        public DelegateCommand OpenHelpCommand { get; }

        public string BangSearchText
        {
            get { return _BangSearchText; }
            set { SetValue(ref _BangSearchText, value); }
        }

        public BangProvider BangProvider
        {
            get;
        }

        private async Task SearchTask()
        {
            try
            {
                var resultUrl = await BangResolver.Resolve(BangSearchText);
                RunWebBrowser(resultUrl);
            }
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", "Error while searching", DialogButtons.Ok);
                _app.Log.Exception(ex);
            }
        }

        private void RunWebBrowser(string url)
        {
            Process p = new Process();
            p.StartInfo.FileName = url;
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private async void Search()
        {
            await SearchTask();
        }

        private async void Ok()
        {
            await SearchTask();
            View.Close();
        }

        private void Cancel()
        {
            View.Close();
        }

        private void OpenHelp()
        {
            RunWebBrowser("https://duckduckgo.com/bang_lite.html");
        }
    }
}
