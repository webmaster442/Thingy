using AppLib.WPF;
using System.Windows.Controls;
using System.Windows.Media;
using Thingy.API;
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
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-note-96.png"); }
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }

        public override UserControl RunModule()
        {
            var view = new Views.Notes.NoteEditor();
            view.DataContext = new ViewModels.Notes.NoteEditorViewModel(view, App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>());
            return view;
        }
    }
}
