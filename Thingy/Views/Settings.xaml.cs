using MahApps.Metro;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Thingy.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccentSelector.SelectedIndex > -1)
            {
                var accent = App.Accents[AccentSelector.SelectedIndex];
                ThemeManager.ChangeAppStyle(Application.Current,
                                  ThemeManager.GetAccent(accent),
                                  ThemeManager.GetAppTheme("BaseLight"));
                Properties.Settings.Default.SelectedAccent = accent;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AccentSelector.ItemsSource = App.Accents;
            AccentSelector.SelectedIndex = Array.IndexOf(App.Accents, Properties.Settings.Default.SelectedAccent);
        }
    }
}
