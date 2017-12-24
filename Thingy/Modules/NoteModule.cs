using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Db;

namespace Thingy.Modules
{
    public class NoteModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "Notes"; }
        }

        public override ImageSource Icon
        {
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-note-96.png")); }
        }

        public override UserControl RunModule()
        {
            return new Views.Note
            {
                DataContext = new ViewModels.NoteViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
        }
    }
}
