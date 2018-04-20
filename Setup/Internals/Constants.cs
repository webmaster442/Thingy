using System;

namespace Setup.Internals
{
    internal static class Constants
    {
        public const string InstallDir = @"%AppDataFolder%\Webmaster442\Thingy";
        public static readonly Guid InstallerGUID = new Guid("6fe30b47-2577-43ad-9085-1861ba25849b");
        public static readonly Guid BootstrapGUID = new Guid("2B1CEB2C-4382-4ABC-9D65-4CCCB120BE93");
        public const string OutputDirectory = @"..\bin\Installer";
        public const string LicenceFile = @"gpl-3.0.rtf";
        public const string FrameworkDloadUrl = @"https://download.microsoft.com/download/8/E/2/8E2BDDE7-F06E-44CC-A145-56C6B9BBE5DD/NDP471-KB4033344-Web.exe";
        public const string FrameworkInstallerFile = @"Net471-web.exe";
        public const string InstallerName = @"thingy-setup";

        public const string Banner = "banner.bmp";
    }
}
