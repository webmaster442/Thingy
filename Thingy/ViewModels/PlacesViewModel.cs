using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Thingy.Db.Entity;
using Thingy.Models;

namespace Thingy.ViewModels
{
    public class PlacesViewModel : ViewModel
    {
        public ObservableCollection<FolderLink> SystemPlaces { get; private set; }
        public ObservableCollection<Drive> Drives { get; private set; }
        

        public PlacesViewModel()
        {
            SystemPlaces = new ObservableCollection<FolderLink>(BuildSystemPlacesList());
            Drives = new ObservableCollection<Drive>(FillDrives());
        }

        private IEnumerable<Drive> FillDrives()
        {
            foreach (var info in DriveInfo.GetDrives())
            {
                yield return new Drive(info);
            }
        }

        private IEnumerable<FolderLink> BuildSystemPlacesList()
        {
            yield return new FolderLink
            {
                Name = "Desktop",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
            };
            yield return new FolderLink
            {
                Name = "Computer",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };
            yield return new FolderLink
            {
                Name = "Documents",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };
            yield return new FolderLink
            {
                Name = "Music",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic)
            };
            yield return new FolderLink
            {
                Name = "Pictures",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };
            yield return new FolderLink
            {
                Name = "Videos",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos)
            };
            yield return new FolderLink
            {
                Name = "Network",
                Path = Environment.GetFolderPath(Environment.SpecialFolder.NetworkShortcuts)
            };
        }
    }
}
