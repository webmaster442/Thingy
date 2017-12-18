using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thingy.ViewModels
{
    public class NoteViewModel: ViewModel
    {
        public NoteViewModel()
        {
            FontSizes = new ObservableCollection<double>(new double[] { 8, 9, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 });
        }

        public ObservableCollection<double> FontSizes
        {
            get;
            private set;
        }

    }
}
