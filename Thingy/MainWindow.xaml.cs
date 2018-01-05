using AppLib.Common.Log;
using AppLib.MVVM;
using Dragablz;
using MahApps.Metro.Controls;
using System;
using System.Windows.Controls;
using Thingy.Properties;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            Title = $"{Title} - {GetAssemblyVersion()}";
            TabControl.ClosingItemCallback = TabClosing;
        }

        private string GetAssemblyVersion()
        {
            var executing = System.Reflection.Assembly.GetExecutingAssembly();
            return executing.GetName().Version.ToString();
        }

        private void TabClosing(ItemActionCallbackArgs<TabablzControl> args)
        {
            var headerModel = args.DragablzItem?.DataContext as HeaderedItemViewModel;
            App.Log.Info($"Closing Tab: {headerModel.Header.ToString()}");
            var viewInTab = headerModel.Content as UserControl;
            if (viewInTab.DataContext is IDisposable viewModel)
            {
                App.Log.Info($"Dispose called for: {viewInTab.DataContext.GetType().FullName}");
                viewModel.Dispose();
            }
            viewInTab.DataContext = null;
            if (viewInTab is IDisposable view)
            {
                App.Log.Info($"Dispose called for: {viewInTab.GetType().FullName}");
                view.Dispose();
            }
            viewInTab = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        public void SetCurrentTabContent(string title, UserControl control, bool newtab)
        {
            MenuFlyout.IsOpen = false; //hide menu
            if (newtab)
            {
                var model = new Dragablz.HeaderedItemViewModel
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
        }

        private void ModernWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
        }

        private void OpenMenu(object sender, System.Windows.RoutedEventArgs e)
        {
            MenuFlyout.IsOpen = true;
        }
    }
}
