using AppLib.WPF;
using System.Collections.Generic;
using System.Linq;
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
            var view = new CoreModules.Views.Notes.NoteEditor();
            view.DataContext = new CoreModules.ViewModels.Notes.NoteEditorViewModel(view, App, App.Resolve<IDataBase>());
            return view;
        }

        private IEnumerable<string> SupportedExtensions
        {
            get
            {
                yield return ".txt";
                yield return ".cpp";
                yield return ".cs";
                yield return ".c";
                yield return ".h";
                yield return ".md";
                yield return ".html";
                yield return ".js";
            }
        }

        public override bool CanHadleFile(string pathOrExtension)
        {
            var extension = System.IO.Path.GetExtension(pathOrExtension);
            return SupportedExtensions.Contains(extension);
        }
    }
}
