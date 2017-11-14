using System.Collections.Generic;
using System.Windows.Controls;
using Thingy.Modules;

namespace Thingy.Infrastructure
{
    public interface IModuleLoader: IEnumerable<IModule>
    {
        UserControl GetModuleByName(string name);
    }
}