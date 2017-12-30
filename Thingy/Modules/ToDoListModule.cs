using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Thingy.Db;
using Thingy.Infrastructure;

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
            get { return new BitmapImage(new Uri("pack://application:,,,/Thingy.Resources;component/Icons/icons8-checklist.png")); }
        }

        public override UserControl RunModule()
        {
            var toDoList = new Views.ToDoList
            {
                DataContext = new ViewModels.ToDoListViewModel(App.Instance, App.IoCContainer.ResolveSingleton<IDataBase>())
            };
            return toDoList;
        }

        public override string Category
        {
            get { return ModuleCategories.Utilities; }
        }
    }
}
