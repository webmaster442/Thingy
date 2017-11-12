using System.Collections.Generic;
using System.Windows.Controls;
using Thingy.Modules;

namespace Thingy.Infrastructure
{
    public interface IModuleLoader: IEnumerable<AbstractModule>
    {
        UserControl GetModuleByName(string name);
    }
}