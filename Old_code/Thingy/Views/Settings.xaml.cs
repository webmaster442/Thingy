using MahApps.Metro;
using System;
using System.Linq;
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
                ThemeManager.ChangeAppStyle(System.Windows.Application.Current,
                                  ThemeManager.GetAccent(accent),
                                  ThemeManager.GetAppTheme("BaseLight"));
                Properties.Settings.Default.SelectedAccent = accent;
            }
        }

        private void BtnRestart_Click(object sender, RoutedEventArgs e)
        {
            App.Instance.Restart();
        }

        private void ActivatorKeyItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.ActivatorKey = ActivatorKeyItems.SelectedItem.ToString();
        }

        private void ActivatorModifierItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.ActivatorModifier1 = ActivatorModifierItems.SelectedItem.ToString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            AccentSelector.ItemsSource = App.Accents;
            AccentSelector.SelectedIndex = Array.IndexOf(App.Accents, Properties.Settings.Default.SelectedAccent);

            var keys = Enum.GetNames(typeof(System.Windows.Forms.Keys));
            ActivatorKeyItems.ItemsSource = keys;
            ActivatorKeyItems.SelectedIndex = Array.IndexOf(keys, Properties.Settings.Default.ActivatorKey);

            var modifiers = Enum.GetNames(typeof(AppLib.Common.ModifierKeys));
            ActivatorModifierItems.ItemsSource = modifiers;
            ActivatorModifierItems.SelectedIndex = Array.IndexOf(modifiers, Properties.Settings.Default.ActivatorModifier1);
        }
    }
}
