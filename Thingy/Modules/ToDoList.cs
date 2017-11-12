using AppLib.Common.IOC;
using System;
using System.Windows.Controls;
using Thingy.Db;

namespace Thingy.Modules
{
    public sealed class ToDoList : AbstractModule
    {
        public override string Name
        {
            get { return "To Do List"; }
        }

        public override Uri IconPath
        {
            get { return null; }
        }

        public override UserControl RunModule()
        {
            var toDoList = new Views.ToDoList();
            toDoList.DataContext = new ViewModels.ToDoListViewModel(App.IoCContainer.ResolveSingleton<IDataBase>());
            return toDoList;
        }
    }
}
