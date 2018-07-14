using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Thingy.API;

namespace Thingy.CoreModules
{
    public static class FontInstaller
    {
        public static async void InstallFonts(IApplication app, IEnumerable<string> fontfiles)
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

                await app.ShowMessageBox("Font Installer", "Fonts Installed succesfully", DialogButtons.Ok);
            }
            catch (Exception ex)
            {
                app.Log.Error(ex);
                await app.ShowMessageBox("Error", "Error installing fonts", DialogButtons.Ok);
            }
        }
    }
}
