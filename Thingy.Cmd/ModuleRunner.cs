using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Thingy.Cmd
{
    internal class ModuleRunner
    {
        private readonly List<IModule> _modules;

        public ModuleRunner()
        {
            _modules = new List<IModule>();
            LoadModules();
        }

        private void LoadModules()
        {
            Assembly current = Assembly.GetAssembly(typeof(IModule));
            var modules = from type in current.GetTypes()
                          where
                          type.IsAssignableFrom(typeof(IModule)) &&
                          !type.IsAbstract && 
                          type.IsClass
                          select type;

            foreach (var module in modules)
            {
                try
                {
                    IModule loaded =  (IModule)Activator.CreateInstance(module);
                    _modules.Add(loaded);
                }
                catch (Exception ex)
                {
#if DEBUG
                    Console.WriteLine(ex);
                    Debugger.Break();
#endif
                }
            }
        }

        public IModule GetModule(string name)
        {
            return _modules.Where(m => m.InvokeName == name).FirstOrDefault();
        }
    }
}
