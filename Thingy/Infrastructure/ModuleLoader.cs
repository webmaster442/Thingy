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
        private List<IModule> _modules;

        public ModuleLoader()
        {
            _modules = new List<IModule>();

            var imodule = typeof(IModule);

            var modulelist = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => imodule.IsAssignableFrom(type) &&
                       type.IsInterface == false && 
                       type.IsAbstract == false);

            foreach (var module in modulelist)
            {
                var instance = (IModule)Activator.CreateInstance(module);
                if (instance.CanLoad)
                {
                    _modules.Add(instance);
                }
            }
        }

        public IEnumerable<IModule> Modules
        {
            get { return _modules; }
        }

        public UserControl GetModuleByName(string name)
        {
            var run = (from module in _modules
                       where module.ModuleName == name
                       select module).FirstOrDefault();

            if (run == null)
                return null;

            return run.RunModule();
        }
    }
}
