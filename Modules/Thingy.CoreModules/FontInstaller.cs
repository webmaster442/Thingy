using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Thingy.CoreModules
{
    public static class FontInstaller
    {
        public static void InstallFonts(IEnumerable<string> fontfiles)
        {
            try
            {
                string temppath = Path.GetTempPath();
                string targetdir = Path.Combine(temppath, "fontinstaller");

                string installerscript = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FontInstaller.vbs");

                if (Directory.Exists(targetdir)) Directory.Delete(targetdir, true);
                if (!Directory.Exists(targetdir)) Directory.CreateDirectory(targetdir);

                foreach (var font in fontfiles)
                {
                    File.Copy(font, Path.Combine(targetdir, Path.GetFileName(font)));
                }

                Process p = new Process();
                p.StartInfo.FileName = "wscript.exe";
                p.StartInfo.Verb = "runas";
                p.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\"", installerscript, targetdir);
                p.Start();

                Thread.Sleep(100);
                Process process = null;

                try
                {
                    do
                    {
                        process = Process.GetProcessById(p.Id);
                    }
                    while (process != null);
                }
                catch (Exception) { }

                Directory.Delete(targetdir, true);

                MessageBox.Show("Fonts Installed succesfully", "Font Installer", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static Task InstallFontsTask(IEnumerable<string> fontfiles)
        {
            return Task.Run(() => InstallFonts(fontfiles));
        }
    }
}
