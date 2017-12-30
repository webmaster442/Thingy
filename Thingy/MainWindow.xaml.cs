using AppLib.MVVM;
using AppLib.WPF.Controls;
using Dragablz;
using MahApps.Metro.Controls;
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
            TabControl.ClosingItemCallback = TabClosing;
        }

        private void TabClosing(ItemActionCallbackArgs<TabablzControl> args)
        {
            var content = args.DragablzItem?.DataContext as HeaderedItemViewModel;
            var closable = content?.Content as ICloseableView;
            closable?.Close();
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
