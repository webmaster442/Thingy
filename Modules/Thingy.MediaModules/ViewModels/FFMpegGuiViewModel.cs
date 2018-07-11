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
        private string _ffmpegPath;
        private BasePreset _preset;
        private IApplication _app;

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

        public string FFMPegFolder
        {
            get { return _ffmpegPath; }
            set
            {
                _ffmpegPath = value;
                _app.Settings.Set("FFMpegPath", value);
            }
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
            FFMPegFolder = _app.Settings.Get("FFMpegPath", "");
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
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@echo off");
            sb.AppendLine("title FFMpeg job");
            sb.AppendFormat("pushd \"{0}\"\r\n", FFMPegFolder);
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
            var name = System.IO.Path.GetTempFileName();
            try
            {
                WriteToFile(name);
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.Arguments = $"/k {name} {name}";
                p.Start();
            }
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
            }
        }

        private async void SaveBach()
        {
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
