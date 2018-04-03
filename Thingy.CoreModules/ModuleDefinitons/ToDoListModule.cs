using AppLib.WPF;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.API;
using Thingy.Db;

namespace Thingy.Modules
{
    public class ToDoListModule : ModuleBase
    {
        public override string ModuleName
        {
            get { return "To Do List"; }
        }

        public override ImageSource Icon
        {
            get { return BitmapHelper.FrozenBitmap("pack://application:,,,/Thingy.Resources;component/Icons/icons8-checklist.png"); }
        }

        public override UserControl RunModule()
        {
            var toDoList = new CoreModules.Views.ToDoList
            {
                DataContext = new ViewModels.ToDoListViewModel(App, App.Resolve<IDataBase>())
            };
            return toDoList;
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
