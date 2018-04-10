using System.Windows.Data;

namespace Thingy.API
{
    public class SettingsBinding: Binding
    {
        public static ISettings Settings { get; set; }

        public SettingsBinding() :base()
        {
            Source = Settings;
            Mode = BindingMode.TwoWay;
        }

        public SettingsBinding(string path): base(path)
        {
            Source = Settings;
            Mode = BindingMode.TwoWay;
        }
    }
}
