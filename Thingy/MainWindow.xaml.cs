using AppLib.WPF.Controls;
using Dragablz;
using System.Windows.Controls;
using System;
using AppLib.MVVM;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
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

        public void SetCurrentTabContent(string title, UserControl control)
        {
            var selected = TabControl.SelectedItem as Dragablz.HeaderedItemViewModel;
            selected.Header = title;
            selected.Content = control;
        }

        private void ModernWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TabablzControl.AddItemCommand.Execute(this, TabControl);
        }
    }
}
