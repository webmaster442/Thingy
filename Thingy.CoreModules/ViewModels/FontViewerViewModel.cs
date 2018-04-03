using AppLib.Common.Extensions;
using AppLib.MVVM;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Thingy.API;
using Thingy.CoreModules;

namespace Thingy.ViewModels
{
    public class FontViewerViewModel: ViewModel
    {
        private IApplication _app; 

        public ObservableCollection<string> FontsToInstall { get; }
        public ObservableCollection<string> FontsToPreview { get; }

        public DelegateCommand AddFontsCommand { get; }
        public DelegateCommand ClearListCommand { get; }
        public DelegateCommand InstallCommand { get; }
        public DelegateCommand OpenDirCommand { get; }

        public FontViewerViewModel(IApplication app)
        {
            _app = app;
            FontsToInstall = new ObservableCollection<string>();
            FontsToPreview = new ObservableCollection<string>();

            AddFontsCommand = Command.ToCommand(AddFonts);
            ClearListCommand = Command.ToCommand(ClearList);
            InstallCommand = Command.ToCommand(Install);
            OpenDirCommand = Command.ToCommand(OpenDir);
        }

        private void OpenDir()
        {
            System.Windows.Forms.FolderBrowserDialog fb = new System.Windows.Forms.FolderBrowserDialog();
            fb.Description = "Select folder to view";
            fb.RootFolder = System.Environment.SpecialFolder.Desktop;
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FontsToPreview.Clear();
                List<string> files = new List<string>();
                files.AddRange(Directory.GetFiles(fb.SelectedPath, "*.ttf"));
                files.AddRange(Directory.GetFiles(fb.SelectedPath, "*.otf"));
                files.Sort();
                FontsToPreview.AddRange(files);
            }
        }

        private async void Install()
        {
            var q = await _app.ShowMessageBox("Question", "Install Fonts?", DialogButtons.YesNo);
            if (q)
            {
                await FontInstaller.InstallFontsTask(FontsToInstall);
            }
        }

        private async void ClearList()
        {
            var q = await _app.ShowMessageBox("Question", "Clear List?", DialogButtons.YesNo);
            if (q)
            {
                FontsToInstall.Clear();
            }
        }

        private void AddFonts()
        {
            System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "Font Files|*.ttf;*.otf";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FontsToInstall.AddRange(ofd.FileNames);
            }
        }
    }
}
