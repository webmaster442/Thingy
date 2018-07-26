using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Thingy.InternalCode
{
    internal static class WebBroswserLocator
    {
        public static IEnumerable<WebBrowser> GetBrowsers()
        {
            var browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet");

            if (browserKeys == null)
            {
                browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");
            }

            var browserNames = browserKeys.GetSubKeyNames();

            foreach (var browserName in browserNames)
            {
                var browserKey = browserKeys.OpenSubKey(browserName);
                var name = browserKey.GetValue(null).ToString();

                RegistryKey registryKey = browserKey.OpenSubKey(@"shell\open\command");
                var path = registryKey.GetValue(null).ToString().StripQuotes();

                var version = "unknown";
                if (!string.IsNullOrEmpty(path))
                {
                    version = FileVersionInfo.GetVersionInfo(path).FileVersion;
                }

                RegistryKey browserIconPath = browserKey.OpenSubKey(@"DefaultIcon");
                var icon = browserIconPath.GetValue(null).ToString().StripQuotes();
                if (!string.IsNullOrEmpty(icon) && icon.Contains(","))
                {
                    var parts = icon.Split(',');
                    icon = parts[0];
                }

                yield return new WebBrowser(name, path, version, icon);
            }

            WebBrowser edgeBrowser = GetEdgeVersion();
            if (edgeBrowser != null)
            {
                yield return edgeBrowser;
            }
        }

        private const string registryEdge = @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Schemas";

        private static WebBrowser GetEdgeVersion()
        {
            RegistryKey edgeKey = Registry.CurrentUser.OpenSubKey(registryEdge);
            if (edgeKey != null)
            {
                string version = edgeKey.GetValue("PackageFullName").ToString().StripQuotes();
                Match result = Regex.Match(version, "(((([0-9.])\\d)+){1})");
                if (result.Success)
                {
                    return new WebBrowser("Edge", "start microsoft-edge:", result.Value, null);
                }
            }
            return null;
        }

        internal static String StripQuotes(this String s)
        {
            if (s.EndsWith("\"") && s.StartsWith("\""))
                return s.Substring(1, s.Length - 2);
            else
                return s;
        }
    }
}
