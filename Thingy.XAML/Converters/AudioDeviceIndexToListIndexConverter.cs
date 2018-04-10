using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Thingy.XAML.Converters
{
    public class AudioDeviceIndexToListIndexConverter: MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            IDictionary<string, int> devices = values[0] as IDictionary<string, int>;

            if (devices == null) return Binding.DoNothing;

            int index = System.Convert.ToInt32(values[1]);

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

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
