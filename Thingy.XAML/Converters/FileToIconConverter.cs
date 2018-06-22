using AppLib.WPF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using System.Linq;

namespace Thingy.XAML.Converters
{
    public class FileToIconConverter : MarkupExtension, IValueConverter
    {
        private Dictionary<string, ImageSource> _cache;

        public FileToIconConverter()
        {
            _cache = new Dictionary<string, ImageSource>();
        }

        private ImageSource Lookup(string path)
        {
            var extension = Path.GetExtension(path);
            if (extension == ".exe" || 
                extension == ".lnk")
            {
                return ExeIconExtractor.GetIcon(path);
            }
            else if (Directory.Exists(path))
            {
                if (Directory.GetLogicalDrives().Contains(path))
                {
                    return ExeIconExtractor.GetIcon(path);
                }
                else
                {
                    return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/SmallIcons/editor/icons8-open-48.png");
                }
            }
            else
            {
                if (_cache.ContainsKey(extension))
                {
                    return _cache[extension];
                }
                else
                {
                    _cache.Add(extension, ExeIconExtractor.GetIcon(path));
                    return _cache[extension];
                }
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var path = value as string;
            if (!string.IsNullOrEmpty(path))
            {
                return Lookup(path);
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
