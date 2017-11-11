using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AppLib.Common;
using AppLib.Common.IOC;
using Thingy.Db;

namespace Thingy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IContainer IoCContainer { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            IoCContainer = new Container();
            IoCContainer.Register<IDataBase, DataBase>(Container.Singleton);
            base.OnStartup(e);
        }
    }
}
