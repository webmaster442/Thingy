using AppLib.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Thingy
{
    public interface IApplication
    {
        bool? ShowDialog(UserControl control, ViewModel model = null);
        void Close();
    }
}
