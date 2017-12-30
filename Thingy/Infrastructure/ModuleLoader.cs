using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using AppLib.Common.Extensions;

namespace Thingy.Infrastructure
{
    public class ModuleLoader : IModuleLoader
    {
        private List<IModule> _modules;
        private Dictionary<string, int> _moudleCounter;

        public ModuleLoader()
        {
            _moudleCounter = new Dictionary<string, int>();
            _moudleCounter.Add("All", 0);
            _modules = new List<IModule>();

            var imodule = typeof(IModule);

            var modulelist = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => imodule.IsAssignableFrom(type) &&
                       type.IsInterface == false && 
                       type.IsAbstract == false);

            foreach (var module in modulelist)
            {
                try
                {
                    var instance = (IModule)Activator.CreateInstance(module);
                    if (instance.CanLoad)
                    {
                        SetCount(instance.Category);
                        _modules.Add(instance);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Module Load error: {0}", ex);
                }
            }
        }

        private void SetCount(string category)
        {
            if (_moudleCounter.ContainsKey(category))
            {
                ++_moudleCounter[category];
            }
            else
            {
                _moudleCounter.Add(category, 1);
            }
            ++_moudleCounter["All"];
        }

        public IDictionary<string, int> CategoryModuleCount
        {
            get { return _moudleCounter;  }
        }

        public IEnumerable<IModule> GetModulesForCategory(string category = null)
        {
            if (string.IsNullOrEmpty(category) || category == "All")
            {
                return _modules.OrderBy(module => module.ModuleName);
            }
            else
            {
                return _modules.Where(module => module.Category == category).OrderBy(module => module.ModuleName);
            }

        }

        public IEnumerable<IModule> GetModulesByName(string searchname)
        {
            if (string.IsNullOrEmpty(searchname))
            {
                return _modules.OrderBy(module => module.ModuleName);
            }
            else
            {
                return _modules.Where(m => m.ModuleName.Contains(searchname, StringComparison.CurrentCultureIgnoreCase)).OrderBy(m => m.ModuleName);
            }
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
