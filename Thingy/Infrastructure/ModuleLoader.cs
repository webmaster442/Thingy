using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using Thingy.Modules;

namespace Thingy.Infrastructure
{
    public class ModuleLoader : IModuleLoader
    {
        private List<IModule> modules;

        public ModuleLoader()
        {
            modules = new List<IModule>();

            var imodule = typeof(IModule);

            var modulelist = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => imodule.IsAssignableFrom(type) && type.IsInterface == false);

            foreach (var module in modulelist)
            {
                modules.Add((IModule)Activator.CreateInstance(module));
            }
        }

        public IEnumerator<IModule> GetEnumerator()
        {
            return modules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return modules.GetEnumerator();
        }

        public UserControl GetModuleByName(string name)
        {
            var run = (from module in modules
                       where module.ModuleName == name
                       select module).FirstOrDefault();

            if (run == null)
                return null;

            return run.RunModule();
        }
    }
}
