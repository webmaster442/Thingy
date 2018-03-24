using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.Implementation
{
    public static class CloudDriveLocation
    {
        public static string OneDrive
        {
            //https://stackoverflow.com/questions/26771265/get-onedrive-path-in-windows
            get
            {
                var key = @"HKEY_CURRENT_USER\Software\Microsoft\OneDrive";
                string oneDrivePath = (string)Registry.GetValue(key, "UserFolder", null);
                return oneDrivePath;
            }
        }

        public static string DropBox
        {
            //https://stackoverflow.com/questions/9660280/how-do-i-programmatically-locate-my-dropbox-folder-using-c
            get
            {
                var infoPath = @"Dropbox\info.json";
                var jsonPath = Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), infoPath);
                if (!File.Exists(jsonPath))
                    jsonPath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), infoPath);
                if (!File.Exists(jsonPath))
                    return null;

                var dropboxPath = File.ReadAllText(jsonPath).Split('\"')[5].Replace(@"\\", @"\");
                return dropboxPath;
            }
        }
    }
}
