using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Thingy.XAML.Converters
{
    public class AudioDeviceIndexToListIndexConverter: MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IDictionary<string, int> devices = parameter as IDictionary<string, int>;

            if (devices == null) return Binding.DoNothing;

            int index = System.Convert.ToInt32(value);
            int currentindex = 0;
            foreach (var keyvalue in devices)
            {
                if (keyvalue.Value == index)
                {
                    return currentindex;
                }
                ++currentindex;
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return -1;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
