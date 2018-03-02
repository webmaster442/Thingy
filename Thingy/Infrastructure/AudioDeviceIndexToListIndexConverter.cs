using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using Thingy.MusicPlayerCore;

namespace Thingy.Infrastructure
{
    public class AudioDeviceIndexToListIndexConverter: MarkupExtension, IValueConverter
    {
        private IDictionary<string, int> _devices;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = System.Convert.ToInt32(value);
            int currentindex = 0;
            foreach (var keyvalue in _devices)
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
            _devices = App.IoCContainer.ResolveSingleton<IAudioEngine>().OutputDevices;
            return this;
        }
    }
}
