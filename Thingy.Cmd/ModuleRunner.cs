using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Thingy.Cmd
{
    internal class ModuleRunner
    {
        private readonly List<ICommandModule> _modules;

        public ModuleRunner()
        {
            _modules = new List<ICommandModule>();
            LoadModules();
        }

        private void LoadModules()
        {
            Assembly current = Assembly.GetAssembly(typeof(ICommandModule));
            var modules = from type in current.GetTypes()
                          where
                          type.GetInterfaces().Contains(typeof(ICommandModule)) &&
                          type.IsInterface == false &&
                          type.IsAbstract == false
                          select type;

            foreach (var module in modules)
            {
                try
                {
                    ICommandModule loaded =  (ICommandModule)Activator.CreateInstance(module);
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

        public ICommandModule GetModule(string name)
        {
            return _modules.Where(m => m.InvokeName == name).FirstOrDefault();
        }
    }
}
