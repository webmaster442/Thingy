using System.Collections.Generic;
using System.Windows.Controls;
using Thingy.Modules;

namespace Thingy.Infrastructure
{
    public interface IModuleLoader
    {
        UserControl RunModuleByName(string name);
        IEnumerable<IModule> GetModulesForCategory(string category = null);
        IEnumerable<IModule> GetModulesByName(string searchname);
        IDictionary<string, int> CategoryModuleCount { get; }
    }
}