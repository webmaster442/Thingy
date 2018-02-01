using System;
using System.IO;
using System.Reflection;

namespace Thingy.Resources
{
    public enum IconCategories
    {
        Small,
        Big,
        Normal
    }

    public static class ResourceLocator
    {
        public static string GetResourceFile(string filename)
        {
            var executing = Assembly.GetAssembly(typeof(ResourceLocator));
            using (Stream stream = executing.GetManifestResourceStream($"Thingy.Resources.{filename}"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static Uri GetIcon(IconCategories category, string name)
        {
            string strcategory;
            switch (category)
            {
                case IconCategories.Small:
                    strcategory = "SmallIcons";
                    break;
                case IconCategories.Big:
                    strcategory = "BigIcons";
                    break;
                case IconCategories.Normal:
                default:
                    strcategory = "Icons";
                    break;
            }

            string final = $"pack://application:,,,/Thingy.Resources;component/{strcategory}/{name}";
            return new Uri(final);
        }
    }
}
