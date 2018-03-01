using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace Thingy.Infrastructure
{
    public class FontFamilyConverter : MarkupExtension, IValueConverter
    {
        private static string GetFontNameFromFile(string filename)
        {
            PrivateFontCollection fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(filename);
            return $"./#{fontCollection.Families[0].Name}";
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Binding.DoNothing;
            string input = System.Convert.ToString(value);
            string name = GetFontNameFromFile(input);
            var uri = new System.Uri(input);
            var ret = new FontFamily(uri, name);
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Binding.DoNothing;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

}
