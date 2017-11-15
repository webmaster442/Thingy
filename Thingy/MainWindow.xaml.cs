using AppLib.WPF.Controls;
using Dragablz;
using System.Windows.Controls;

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
