using AppLib.Common;
using Dragablz;
using MahApps.Metro.Controls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shell;
using Thingy.API;
using Thingy.API.Capabilities;
using Thingy.Infrastructure;
using Thingy.Properties;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IMainWindow
    {
        private IApplication _app;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(IApplication app): this()
        {
            _app = app;
            StatusBar.Application = _app;
            DataContext = new MainWindowViewModel(this, app);
            Title = $"{Title} - {GetAssemblyVersion()}";
            TabControl.ClosingItemCallback = TabClosing;
        }

        public UserControl CurrentTabContent
        {
            get
            {
                var selected = TabControl.SelectedItem as HeaderedItemViewModel;
                return selected.Content as UserControl;
            }
        }

        public int TabCount
        {
            get { return TabControl.Items.Count; }
        }

        public void ShowHideMenu()
        {
            MenuFlyout.IsOpen = !MenuFlyout.IsOpen;
        }

        private string GetAssemblyVersion()
        {
            var executing = System.Reflection.Assembly.GetExecutingAssembly();
            return executing.GetName().Version.ToString();
        }

        internal void CloseCurrentTab()
        {
            _app.Log.Info("Current Tab was closed from code");
            var tabs = TabControl.GetOrderedHeaders().ToList();
            var curenttab = tabs[TabControl.SelectedIndex]; 
            TabablzControl.CloseItemCommand.Execute(curenttab, TabControl);
        }

        private async void TabClosing(ItemActionCallbackArgs<TabablzControl> args)
        {
            var headerModel = args.DragablzItem?.DataContext as HeaderedItemViewModel;
            _app.Log.Info($"Closing Tab: {headerModel.Header.ToString()}");
            var viewInTab = headerModel.Content as UserControl;

            Guid moduleId = Guid.Empty;

            if (viewInTab.Tag != null)
            {
                moduleId = Guid.Parse(viewInTab?.Tag.ToString());
            }

            if (viewInTab.DataContext is IDisposable viewModel)
            {
                _app.Log.Info($"Dispose called for: {viewInTab.DataContext.GetType().FullName}");
                viewModel.Dispose();
            }
            viewInTab.DataContext = null;

            if (viewInTab is IHaveCloseTask taskView)
            {
                await taskView.ClosingTask();
            }

            if (viewInTab is IDisposable view)
            {
                _app.Log.Info($"Dispose called for: {viewInTab.GetType().FullName}");
                view.Dispose();
            }
            viewInTab = null;

            if (moduleId != Guid.Empty)
            {
                _app.TabManager.ModuleClosed(moduleId);
            }

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void SetCurrentTabContent(string title, UserControl control, bool newtab)
        {
            MenuFlyout.IsOpen = false; //hide menu
            if (newtab)
            {
                var model = new HeaderedItemViewModel
                {
                    Header = title,
                    Content = control
                };
                int index = TabControl.Items.Add(model);
                TabControl.SelectedIndex = index;
            }
            else
            {
                var selected = TabControl.SelectedItem as HeaderedItemViewModel;
                selected.Header = title;
                selected.Content = control;
            }
        }

        public int FindTabByTitle(string title)
        {
            int counter = 0;
            if (TabControl == null || TabControl.Items.Count < 1) return -1;
            foreach (HeaderedItemViewModel model in TabControl?.Items)
            {
                if (model.Header.ToString() == title)
                {
                    return counter;
                }
                ++counter;
            }
            return -1;
        }

        public void FocusTabByIndex(int index)
        {
            TabControl.SelectedIndex = index;
        }

        private void ModernWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TabablzControl.AddItemCommand.Execute(this, TabControl);
            FocusTabByIndex(0);
            Program.CommandLineParser.Parse(Environment.CommandLine);
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void StatusFlyOut_ClosingFinished(object sender, System.Windows.RoutedEventArgs e)
        {
            _app.Log.Info("Closing Status flyout");
            if (StatusFlyOut.Content != null)
            {
                if (StatusFlyOut.Content is IDisposable disposable)
                {
                    _app.Log.Info($"Dispodsing type: {StatusFlyOut.Content.GetType().FullName}");
                    disposable.Dispose();
                }
                StatusFlyOut.Content = null;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.SystemKey == Key.LeftAlt) ShowHideMenu();
            base.OnKeyDown(e);
        }

        private void Window_Drop(object sender, System.Windows.DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                _app.HandleFiles(files);
            }
        }

        private void Window_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        public void SetBusyOverlayVisibility(bool isVisible)
        {
            TaskbarItemInfo.ProgressState = isVisible ? TaskbarItemProgressState.Indeterminate : TaskbarItemProgressState.None;
            BusyOverlay.Visibility = isVisible ? Visibility.Visible : Visibility.Collapsed;
            BusyProgressRing.IsActive = isVisible;
        }
    }
}
