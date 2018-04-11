using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.API;

namespace Thingy.Implementation
{
    internal interface IModuleLoader
    {
        IModule GetModuleByName(string name);
        IEnumerable<IModule> GetModulesForCategory(string category = null);
        IEnumerable<IModule> GetModulesByName(string searchname);
        IDictionary<string, int> CategoryModuleCount { get; }
        IModule GetModuleForFile(string file);
    }
}
