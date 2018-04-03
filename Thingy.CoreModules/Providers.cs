using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media.Imaging;
using Thingy.CoreModules.Models;

namespace Thingy.CoreModules
{
    public static class Providers
    {
        public static IEnumerable<Drive> ProvideDriveData()
        {
            foreach (var info in DriveInfo.GetDrives())
            {
                if (info.IsReady)
                {
                    yield return new Drive(info);
                }
            }
        }

        public static IEnumerable<SystemFolderLink> ProvideSystemPlaces()
        {
            yield return new SystemFolderLink
            {
                Name = "Desktop",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-desktop.png"))

            };
            yield return new SystemFolderLink
            {
                Name = "Computer",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-computer.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Network",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-wired-network.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Home",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-user-folder.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Documents",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-documents-folder.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Music",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-music-folder.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Pictures",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-pictures-folder.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Videos",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-movies-folder.png"))
            };
            yield return new SystemFolderLink
            {
                Name = "Downloads",
                Path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"),
                Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-downloads-folder.png"))
            };
            if (!string.IsNullOrEmpty(CloudDriveLocation.DropBox))
            {
                yield return new SystemFolderLink
                {
                    Name = "Dropbox",
                    Path = CloudDriveLocation.DropBox,
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-dropbox.png"))
                };
            }
            if (!string.IsNullOrEmpty(CloudDriveLocation.OneDrive))
            {
                yield return new SystemFolderLink
                {
                    Name = "OneDrive",
                    Path = CloudDriveLocation.OneDrive,
                    Icon = new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-onedrive.png"))
                };
            }
        }
    }
}
