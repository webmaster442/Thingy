using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Thingy.Implementation.Bang
{
    internal class BangProvider
    {
        public List<string> Bangs { get; }
        public List<string> Descriptions { get; }

        public BangProvider()
        {
            Bangs = new List<string>(11414);
            Descriptions = new List<string>(11414);

            var asm = Assembly.GetAssembly(typeof(BangProvider));
            using (Stream stream = asm.GetManifestResourceStream("Thingy.Implementation.BangProvider.Bangs.txt"))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line = null;
                    do
                    {
                        line = reader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            var columns = line.Split(';');
                            Bangs.Add(columns[1]);
                            Descriptions.Add(columns[0]);
                        }
                    }
                    while (!string.IsNullOrEmpty(line));
                }
            }
        }
    }
}
