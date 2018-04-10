using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;

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
    }
}
