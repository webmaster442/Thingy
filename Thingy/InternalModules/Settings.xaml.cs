using AppLib.Common.Extensions;
using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Thingy.API;

namespace Thingy.InternalModules
{
    public class SettingModell: BindableBase
    {
        private string _key;
        private string _value;

        public string Key
        {
            get { return _key; }
            set { SetValue(ref _key, value); }
        }

        public string Value
        {
            get { return _value; }
            set { SetValue(ref _value, value); }
        }
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
            StoredSettings = new ObservableCollection<SettingModell>();
        }

        public Settings(IApplication app): this()
        {
            _app = app;
        }

        public ObservableCollection<SettingModell> StoredSettings { get; }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SettingModell> diff = new List<SettingModell>();
                foreach (var setting in _app.Settings)
                {
                    var vm = (from i in StoredSettings
                             where i.Key == setting.Key
                             select i).FirstOrDefault();

                    if (vm != null && vm.Value != setting.Value)
                    {
                        diff.Add(vm);
                    }
                }

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
