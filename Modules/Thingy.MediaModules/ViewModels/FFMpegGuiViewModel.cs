using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Text;
using Thingy.API;
using Thingy.MediaModules.Controls;
using Thingy.MediaModules.Models;

namespace Thingy.MediaModules.ViewModels
{
    public class FFMpegGuiViewModel : ViewModel
    {
        private string _generated;
        private BasePreset _preset;
        private IApplication _app;
        private bool _OutputOk;

        public PresetList Presets
        {
            get;
            private set;
        }

        public ObservableCollection<string> Files
        {
            get;
            private set;
        }

        public ObservableCollection<Tuple<string, string>> FileTable
        {
            get;
            private set;
        }

        public BasePreset SelectedPreset
        {
            get { return _preset; }
            set { SetValue(ref _preset, value); }
        }

        public string GeneratedBach
        {
            get { return _generated; }
            set { SetValue(ref _generated, value); }
        }

        public bool OutputOk
        {
            get { return _OutputOk; }
            set { SetValue(ref _OutputOk, value); }
        }

        public DelegateCommand AddFilesCommand { get; private set; }
        public DelegateCommand<string> RemoveSelectedCommand { get; private set; }
        public DelegateCommand ClearListCommand { get; private set; }
        public DelegateCommand GenerateBachCommand { get; private set; }
        public DelegateCommand SaveBachCommand { get; private set; }
        public DelegateCommand ExecuteBachCommand { get; private set; }

        public FFMpegGuiViewModel(IApplication app)
        {
            Presets = new PresetList();
            _app = app;
            Files = new ObservableCollection<string>();
            FileTable = new ObservableCollection<Tuple<string, string>>();
            AddFilesCommand = Command.CreateCommand(AddFiles);
            RemoveSelectedCommand = Command.CreateCommand<string>(RemoveSelected, CanRemove);
            ClearListCommand = Command.CreateCommand(ClearList);
            GenerateBachCommand = Command.CreateCommand(GenerateBach);
            SaveBachCommand = Command.CreateCommand(SaveBach);
            ExecuteBachCommand = Command.CreateCommand(ExecuteBach);
        }

        private void AddFiles()
        {
            var sfd = new System.Windows.Forms.OpenFileDialog();
            sfd.Multiselect = true;
            sfd.Filter = "All files|*.*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Files.AddRange(sfd.FileNames);
            }
        }

        private void RemoveSelected(string obj)
        {
            Files.Remove(obj);
        }

        private bool CanRemove(string obj)
        {
            return !string.IsNullOrEmpty(obj);
        }

        private void ClearList()
        {
            Files.Clear();
        }

        private void GenerateBach()
        {
            string ffmpeg = _app.Settings.Get("FFMpegPath", string.Empty);

            if (string.IsNullOrEmpty(ffmpeg))
            {
                GeneratedBach = "FFMpeg not found. Job can't be created.";
                return;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@echo off");
            sb.AppendLine("title FFMpeg job");
            sb.AppendFormat("pushd \"{0}\"\r\n", System.IO.Path.GetDirectoryName(ffmpeg));
            if (SelectedPreset != null)
            {
                foreach (var entry in FileTable)
                {
                    SelectedPreset.InputFile = entry.Item1;
                    SelectedPreset.OutputFile = entry.Item2;
                    sb.AppendLine(SelectedPreset.CommandLine);
                }
            }
            sb.AppendLine("popd");
            GeneratedBach = sb.ToString();
        }

        private void WriteToFile(string filename)
        {
            using (var fs = System.IO.File.CreateText(filename))
            {
                fs.Write(GeneratedBach);
            }
        }

        private async void ExecuteBach()
        {
            var name = $"{System.IO.Path.GetTempFileName()}.bat";
            try
            {
                bool @continue = true;
                if (!OutputOk)
                {
                    @continue = await _app.ShowMessageBox("Warning", "Output folder not set. Continue?", DialogButtons.YesNo);
                }
                if (@continue)
                {
                    WriteToFile(name);
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = $"/k \"{name}\"";
                    p.Start();
                }
            }
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
            }
        }

        private async void SaveBach()
        {
            bool @continue = true;
            if (!OutputOk)
            {
                @continue = await _app.ShowMessageBox("Warning", "Output folder not set. Continue?", DialogButtons.YesNo);
            }
            if (!@continue) return;
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "CMD file|*.cmd";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    WriteToFile(sfd.FileName);
                }
                catch (Exception ex)
                {
                    await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
                }
            }
        }

    }
}
