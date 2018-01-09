using System.Collections.Generic;

namespace Thingy.CalculatorCore.Constants
{
    internal interface IConstantProvider
    {
        string Category { get; }
        IEnumerable<Constant> Constants { get; }
    }

    public interface IConstantDB
    {
        IEnumerable<string> Categories { get; }
        IEnumerable<Constant> RecentlyUsed { get; }
        IEnumerable<Constant> SearchByName(string name);
        IEnumerable<Constant> GetCategory(string category);
        Constant Lookup(string name);
    }
}
