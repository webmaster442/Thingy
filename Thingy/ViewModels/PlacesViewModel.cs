using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Thingy.Db.Entity;
using Thingy.Implementation;
using Thingy.Implementation.Models;

namespace Thingy.ViewModels
{
    public class PlacesViewModel : ViewModel
    {
        public ObservableCollection<SystemFolderLink> SystemPlaces { get; private set; }
        public ObservableCollection<Drive> Drives { get; private set; }
        

        public PlacesViewModel()
        {
            SystemPlaces = new ObservableCollection<SystemFolderLink>(Providers.ProvideSystemPlaces());
            Drives = new ObservableCollection<Drive>(Providers.ProvideDriveData());
        }


    }
}
