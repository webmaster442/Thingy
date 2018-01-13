using AppLib.WPF.Converters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Thingy.Controls
{
    public class UniformGridColunConverter : ConverterBase<UniformGridColunConverter>, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double input = System.Convert.ToDouble(value);
                var columns = Math.Floor((input - 10) / 90);
                if (columns > 10)
                    return 10;
                else
                    return columns;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
