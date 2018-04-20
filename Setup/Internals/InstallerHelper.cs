using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Setup.Internals
{
    internal static class InstallerHelper
    {
        public static string ThingyReleaseDirectory
        {
            get
            {
                var current = AppDomain.CurrentDomain.BaseDirectory;
                return current.Replace("installer", "release");
            }
        }

        public static string OutputInstaller
        {
            get
            {
                return Path.Combine(Constants.OutputDirectory, Constants.InstallerName + ".exe");
            }
        }

        public static Version GetThingyVersion()
        {
            var thingy = Path.Combine(ThingyReleaseDirectory, "thingy.exe");
            if (File.Exists(thingy))
            {
                Assembly a = Assembly.LoadFile(thingy);
                var version = a.GetName().Version;
                return new Version(version.Major - 2000, version.Minor, version.Build, version.Revision);
            }
            else
            {
                var major = DateTime.Now.Year - 2000;
                return new Version(major, DateTime.Now.Month, DateTime.Now.Day);
            }
        }

        public static WixSharp.WixEntity[] GetFiles(string source, params string[] filterextensions)
        {
            var currentDirectory = new DirectoryInfo(source);
            var subDirectories = currentDirectory.GetDirectories();
            var files = currentDirectory.GetFiles();
            var entities = new List<WixSharp.WixEntity>();

            if (filterextensions.Any())
            {
                foreach (var file in files)
                {
                    if (filterextensions.Contains(file.Extension))
                        entities.Add(new WixSharp.File(currentDirectory.FullName + @"\" + file.Name));

                    Console.WriteLine("Added file: {0}", currentDirectory.FullName + @"\" + file.Name);
                }
                foreach (var directory in subDirectories)
                {
                    entities.Add(new WixSharp.Dir(directory.Name, GetFiles(directory.FullName, filterextensions)));
                }
            }
            else
            {
                foreach (var file in files)
                {
                    entities.Add(new WixSharp.File(currentDirectory.FullName + @"\" + file.Name));
                    Console.WriteLine("Added file: {0}", currentDirectory.FullName + @"\" + file.Name);
                }
                foreach (var directory in subDirectories)
                {
                    entities.Add(new WixSharp.Dir(directory.Name, GetFiles(directory.FullName)));
                }
            }
            return entities.ToArray();
        }
    }
}
