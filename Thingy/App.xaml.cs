using AppLib.Common.IOC;
using MahApps.Metro;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Thingy.Db;
using Thingy.Infrastructure;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, IApplication
    {
        public static AppLib.Common.IOC.IContainer IoCContainer { get; private set; }
        public static string[] Accents { get; private set; }

        public static IApplication Instance
        {
            get { return App.Current as IApplication; }
        }

        public void Close()
        {
            App.Current.Shutdown();
        }

        public int FindTabByTitle(string Title)
        {
            return (App.Current.MainWindow as MainWindow).FindTabByTitle(Title);
        }

        public void FocusTabByIndex(int index)
        {
            (App.Current.MainWindow as MainWindow).FocusTabByIndex(index);
        }

        public void OpenTabContent(string Title, UserControl control)
        {
            (App.Current.MainWindow as MainWindow).SetCurrentTabContent(Title, control, true);
        }

        public void SetCurrentTabContent(string Title, UserControl control)
        {
            (App.Current.MainWindow as MainWindow).SetCurrentTabContent(Title, control, false);
        }

        public bool? ShowDialog(UserControl control, string Title, INotifyPropertyChanged model = null)
        {
            ModalDialog modalDialog = new ModalDialog();
            if (model != null)
                control.DataContext = model;
            modalDialog.DailogContent = control;
            modalDialog.Title = Title;
            modalDialog.Owner = App.Current.MainWindow;
            return modalDialog.ShowDialog();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Accents = new string[]
            {
                "Red", "Green", "Blue",
                "Purple", "Orange", "Lime",
                "Emerald", "Teal", "Cyan",
                "Cobalt", "Indigo", "Violet",
                "Pink", "Magenta", "Crimson",
                "Amber", "Yellow", "Brown",
                "Olive", "Steel", "Mauve",
                "Taupe", "Sienna"
            };

            IoCContainer = new AppLib.Common.IOC.Container();
            IoCContainer.RegisterSingleton<IDataBase>(() =>
            {
                return new DataBase("test.db");
            });
            IoCContainer.RegisterSingleton<IModuleLoader, ModuleLoader>();

            var accent = Thingy.Properties.Settings.Default.SelectedAccent;
            ThemeManager.ChangeAppStyle(Application.Current,
                              ThemeManager.GetAccent(accent),
                              ThemeManager.GetAppTheme("BaseLight"));

            base.OnStartup(e);
        }
    }
}
