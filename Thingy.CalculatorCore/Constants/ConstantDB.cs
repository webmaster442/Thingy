using AppLib.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Thingy.CalculatorCore.Constants
{
    public class ConstantDB: IConstantDB
    {
        private HashSet<Constant> _recent;
        private Dictionary<string, IEnumerable<Constant>> _db;

        public ConstantDB()
        {
            _db = new Dictionary<string, IEnumerable<Constant>>();
            _recent = new HashSet<Constant>();
            FillDB();
        }

        public IEnumerable<string> Categories
        {
            get { return _db.Keys; }
        }

        public IEnumerable<Constant> RecentlyUsed
        {
            get { return _recent; }
        }

        public IEnumerable<Constant> GetCategory(string category)
        {
            return _db[category];
        }

        public Constant Lookup(string name)
        {
            var q = from subCollection in _db.AllValues()
                    from constant in subCollection
                    where constant.Name == name
                    orderby constant.Name
                    select constant;

            return q.FirstOrDefault();
        }

        public IEnumerable<Constant> SearchByName(string name)
        {
            var q = from subCollection in _db.AllValues()
                    from constant in subCollection
                    where constant.Name.StartsWith(name, StringComparison.InvariantCultureIgnoreCase)
                    orderby constant.Name
                    select constant;

            return q;
        }

        private void FillDB()
        {
            var iprovider = typeof(IConstantProvider);
            var assembly = Assembly.GetAssembly(iprovider);
            var providers = assembly.GetTypes()
                           .Where(type => iprovider.IsAssignableFrom(type) &&
                                          type.IsInterface == false &&
                                          type.IsAbstract == false);
            foreach (var provider in providers)
            {
                var instance = (IConstantProvider)Activator.CreateInstance(provider);
                _db.Add(instance.Category, instance.Constants);
            }
        }
    }
}
