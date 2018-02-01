using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Thingy.MusicPlayerCore.DataObjects;

namespace Thingy.MusicPlayerCore.Controls
{
    public class PlaylistTagConverter : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// Get a value from a dictionary. If value is not present, then the fallback value is returned
        /// </summary>
        /// <typeparam name="Tkey">key type</typeparam>
        /// <typeparam name="TValue">value type</typeparam>
        /// <param name="dictionary">dictionary to add to or update</param>
        /// <param name="key">key parameter</param>
        /// <param name="fallback">fallback value</param>
        /// <returns></returns>
        public static TValue GetValueOrFallback<Tkey, TValue>(Dictionary<Tkey, TValue> dictionary, Tkey key, TValue fallback)
        {
            if (dictionary.ContainsKey(key)) return dictionary[key];
            else return fallback;
        }

        /// <summary>
        /// Converts a full path to a file name
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>string, filename</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fullpath = value.ToString();

            if (fullpath.StartsWith("http"))
            {
                return fullpath;
            }
            else if (fullpath.StartsWith("cd:"))
            {
                try
                {
                    var cd = new CDTrackInfo(fullpath);
                    if (CDInfoProvider.CdData.Count > 0)
                    {
                        var title = GetValueOrFallback(CDInfoProvider.CdData, $"TITLE{cd.Track + 1}", "Unknown song");
                        var artist = GetValueOrFallback(CDInfoProvider.CdData, $"PERFORMER{cd.Track + 1}", "Unknown artist");
                        return $"{artist} - {title}";
                    }
                    else return $"CD track {cd.Track+1}, on Drive: {cd.Drive}";
                }
                catch (Exception)
                {
                    return fullpath;
                }
            }
            else
            {
                try
                {
                    TagLib.File tags = TagLib.File.Create(fullpath);
                    var artist = tags.Tag.Performers[0];
                    var title = tags.Tag.Title;
                    if (string.IsNullOrEmpty(artist)) artist = "Unknown artist";
                    if (string.IsNullOrEmpty(title)) title = "Unknown song";
                    return string.Format("{0} - {1}", artist, title);

                }
                catch (Exception)
                {
                    var fname = System.IO.Path.GetFileName(fullpath);
                    return fname;
                }
            }
        }

        /// <summary>
        /// Returns the unmodified input
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>unmodified inpt</returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
