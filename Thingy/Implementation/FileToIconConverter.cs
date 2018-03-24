using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Thingy.InternalCode;

namespace Thingy.Implementation
{
    public class FileToIconConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            if (!string.IsNullOrEmpty(path))
            {
                return ExeIconExtractor.GetIcon(path);
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
