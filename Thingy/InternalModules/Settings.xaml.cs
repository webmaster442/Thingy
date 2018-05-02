using AppLib.Common.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;

namespace Thingy.InternalModules
{
    public class SettingModell
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

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

        public ObservableCollection<SettingModell> StoredSettings;

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                await _app.ShowMessageBox("Error", ex.Message, DialogButtons.Ok);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TabControl.SelectedIndex != 1) return;

            StoredSettings = new ObservableCollection<SettingModell>();
            var query = from setting in _app.Settings
                        where !string.IsNullOrEmpty(setting.Key)
                        select new SettingModell
                        {
                            Key = setting.Key,
                            Value = setting.Value
                        };

            StoredSettings.UpdateWith(query);

            SettingsGrid.ItemsSource = StoredSettings;
        }
    }
}
