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
        private HashSet<AbstractModule> modules;

        public ModuleLoader()
        {
            modules = new HashSet<AbstractModule>();
            var assembly = Assembly.GetAssembly(typeof(AbstractModule));

            var modulelist = from module in assembly.GetTypes()
                          where module.IsAssignableFrom(typeof(AbstractModule))
                          select (AbstractModule)Activator.CreateInstance(module);

            foreach (var module in modulelist)
            {
                modules.Add(module);
            }
        }

        public IEnumerator<AbstractModule> GetEnumerator()
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
                       where module.Name == name
                       select module).FirstOrDefault();

            if (run == null)
                return null;

            return run.RunModule();
        }
    }
}
