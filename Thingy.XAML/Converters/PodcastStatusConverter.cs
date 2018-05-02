using AppLib.WPF;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Thingy.XAML.Converters
{
    public class PodcastStatusConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var file = value as string;
            if (string.IsNullOrEmpty(file))
            {
                return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-downloading-updates-96.png");
            }
            else
            {
                if (!System.IO.File.Exists(file))
                    return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-downloading-updates-96.png");
                else
                    return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-play-96.png");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
