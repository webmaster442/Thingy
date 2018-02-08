using AppLib.Common.Log;
using AppLib.MVVM;
using Dragablz;
using System;
using System.Threading.Tasks;
using Thingy.Infrastructure;

namespace Thingy
{
    public class MainWindowViewModel: ViewModel<IMainWindow>
    {
        public DelegateCommand SettingCommand { get; private set; }
        public DelegateCommand ExitCommand { get; private set; }
        public DelegateCommand LogCommand { get; private set; }
        public DelegateCommand OpenMenuCommand { get; private set; }
        public DelegateCommand AboutCommand { get; private set; }

        public DelegateCommand ModuleImportCommand { get; private set; }
        public DelegateCommand ModuleAppendCommand { get; private set; }
        public DelegateCommand ModuleExportCommand { get; private set; }

        private ILogger _log;
        private IApplication _app;

        public MainWindowViewModel(IMainWindow view, IApplication app, ILogger log): base(view)
        {
            _app = app;
            _log = log;
            SettingCommand = Command.ToCommand(Setting, CanOpenSetting);
            ExitCommand = Command.ToCommand(Exit);
            LogCommand = Command.ToCommand(Log);
            OpenMenuCommand = Command.ToCommand(OpenMenu);
            AboutCommand = Command.ToCommand(OpenAbout);
            ModuleImportCommand = Command.ToCommand(ModuleImport, CanImportExport);
            ModuleExportCommand = Command.ToCommand(ModuleExport, CanImportExport);
            ModuleAppendCommand = Command.ToCommand(ModuleAppend, CanImportExport);
        }

        private bool CanImportExport()
        {
            return View?.CurrentTabContent?.DataContext is ICanImportExportXMLData;
        }

        private async void ModuleExport()
        {
            var sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.Filter = "XML|*.xml";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var module = View?.CurrentTabContent?.DataContext as ICanImportExportXMLData;
                    if (module != null)
                    {
                        using (var file = System.IO.File.OpenWrite(sfd.FileName))
                        {
                            View.SetBusyOverlayVisibility(true);
                            await module.Export(file);
                            View.SetBusyOverlayVisibility(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    View.SetBusyOverlayVisibility(false);
                    await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                }
            }
        }

        private async Task DoImport(bool append)
        {
            var ofd = new System.Windows.Forms.OpenFileDialog();
            ofd.Filter = "XML|*.xml";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    var module = View?.CurrentTabContent?.DataContext as ICanImportExportXMLData;
                    if (module != null)
                    {
                        using (var file = System.IO.File.OpenWrite(ofd.FileName))
                        {
                            View.SetBusyOverlayVisibility(true);
                            await module.Import(file, append);
                            View.SetBusyOverlayVisibility(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    View.SetBusyOverlayVisibility(false);
                    await _app.ShowMessageBox("Error", ex.Message, MahApps.Metro.Controls.Dialogs.MessageDialogStyle.Affirmative);
                }
            }
        }

        private async void ModuleImport()
        {
            await DoImport(false);
        }

        private async void ModuleAppend()
        {
            await DoImport(true);
        }

        private void OpenAbout()
        {
            _app.TabManager.CreateNewTabContent("About", new Views.About());
        }

        private void OpenMenu()
        {
            View.ShowHideMenu();
        }

        private void Log()
        {
            var logviewer = new AppLib.WPF.Dialogs.LogViewer
            {
                Log = _log
            };
            _app.ShowDialog(logviewer, "Application Log");
        }

        private bool CanOpenSetting()
        {
            int index = _app.TabManager.GetTabIndexByTitle("Settings");
            return index == -1;
        }

        private void Setting()
        {
            _app.TabManager.CreateNewTabContent("Settings", new Views.Settings());
        }

        private void Exit()
        {
            App.Current.Shutdown();
        }

        public Func<HeaderedItemViewModel> ItemFactory
        {
            get
            {
                return () =>
                {
                    var start = new Views.StartPage
                    {
                        DataContext = new ViewModels.StartPageViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IModuleLoader>())
                    };

                    return new HeaderedItemViewModel
                    {
                        IsSelected = true,
                        Header = "New Tool...",
                        Content = start
                    };
                };
            }
        }
    }
}
