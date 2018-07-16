using System;
using System.Diagnostics;
using AppLib.MVVM;
using Thingy.API;

namespace Thingy.MediaModules.ViewModels
{
    public class MpvPlayerViewModel: ViewModel
    {
        private readonly IApplication _app;

        private string _file;
        private string _parameters;

        public MpvPlayerViewModel(IApplication app)
        {
            _app = app;
            ClearParametersCommand = Command.CreateCommand(ClearParameters, CanClearParameters);
            AddOptionCommand = Command.CreateCommand<string>(AddOption, CanAddOption);
            Parameters = string.Empty;
            File = string.Empty;
        }

        public string Parameters
        {
            get { return _parameters; }
            set { SetValue(ref _parameters, value); }
        }

        public string File
        {
            get { return _file; }
            set
            {
                SetValue(ref _file, value);
                YoutubeOptionsAvailable = Formats.IsYoutubeUrl(value);
                OnPropertyChanged(() => YoutubeOptionsAvailable);
            }
        }

        public bool YoutubeOptionsAvailable
        {
            get;
            set;
        }

        public DelegateCommand ClearParametersCommand { get; }
    
        public DelegateCommand<string> AddOptionCommand { get; }

        private bool CanAddOption(string obj)
        {
            return !Parameters.Contains(obj);
        }

        private void AddOption(string obj)
        {
             Parameters = $"{Parameters.TrimEnd()} {obj}";
        }

        private bool CanClearParameters()
        {
            return !string.IsNullOrEmpty(Parameters);
        }

        private void ClearParameters()
        {
            Parameters = string.Empty;
        }

        internal void StartPlayer()
        {
            try
            {
                if (string.IsNullOrEmpty(File)) return;

                var defaultmpv = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Apps\x64\mpv.exe");
                var mpv = _app.Settings.Get("MPVPath", defaultmpv);

                Process p = new Process();
                p.StartInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(mpv);
                p.StartInfo.FileName = "mpv.exe";

                if (string.IsNullOrEmpty(Parameters))
                    p.StartInfo.Arguments = $"\"{File}\"";
                else
                    p.StartInfo.Arguments = $"{Parameters} \"{File}\"";

                _app.Log.Info("Starting mpv with parameters: {0}", p.StartInfo.Arguments);
                p.Start();
            }
            catch (Exception ex)
            {
                _app.Log.Error(ex);
            }
        }
    }
}
