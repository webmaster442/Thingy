using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;
using System.Linq;

namespace Thingy.InternalModules
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        private readonly IApplication _app;

        public Settings()
        {
            InitializeComponent();
        }

        public Settings(IApplication app): this()
        {
            _app = app;
        }

        public ObservableCollection<KeyValuePair<string, string>> StoredSettings;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StoredSettings = new ObservableCollection<KeyValuePair<string, string>>(_app.Settings);
            SettingsGrid.ItemsSource = StoredSettings;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var diff = from vm in StoredSettings
                       from setting in _app.Settings
                       where
                       vm.Key == setting.Key &&
                       vm.Value != setting.Value
                       select vm;

            foreach (var d in diff)
            {
                _app.Settings[d.Key, null] = d.Value;
            }
        }
    }
}
