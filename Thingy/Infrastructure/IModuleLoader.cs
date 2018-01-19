using System.Collections.Generic;

namespace Thingy.Infrastructure
{
    public interface IModuleLoader
    {
        IModule GetModuleByName(string name);
        IEnumerable<IModule> GetModulesForCategory(string category = null);
        IEnumerable<IModule> GetModulesByName(string searchname);
        IDictionary<string, int> CategoryModuleCount { get; }
    }
}