using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Text;
using Thingy.FFMpegGui;

namespace Thingy.ViewModels
{
    public class FFMpegGuiViewModel : ViewModel
    {
        private string _generated;
        private Preset _preset;
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

        public Preset SelectedPreset
        {
            get { return _preset; }
            set { SetValue(ref _preset, value); }
        }

        public string GeneratedBach
        {
            get { return _generated; }
            set { SetValue(ref _generated, value); }
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
            AddFilesCommand = Command.ToCommand(AddFiles);
            RemoveSelectedCommand = Command.ToCommand<string>(RemoveSelected, CanRemove);
            ClearListCommand = Command.ToCommand(ClearList);
            GenerateBachCommand = Command.ToCommand(GenerateBach);
            SaveBachCommand = Command.ToCommand(SaveBach);
            ExecuteBachCommand = Command.ToCommand(ExecuteBach);
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
            foreach (var entry in FileTable)
            {
                SelectedPreset.InputFile = entry.Item1;
                SelectedPreset.OutputFile = entry.Item2;
                sb.AppendLine(SelectedPreset.CommandLine);
            }
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
                await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
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
                    await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                }
            }
        }

    }
}
