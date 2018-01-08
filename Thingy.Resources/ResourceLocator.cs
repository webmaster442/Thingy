using System.IO;
using System.Reflection;

namespace Thingy.Resources
{
    public static class ResourceLocator
    {
        public static string Get(string filename)
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
    }
}
