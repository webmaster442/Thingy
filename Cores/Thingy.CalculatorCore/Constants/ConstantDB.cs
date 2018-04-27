using AppLib.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Thingy.CalculatorCore.Constants
{
    public class ConstantDB : IConstantDB
    {
        private HashSet<Constant> _recent;
        private Dictionary<string, IEnumerable<Constant>> _db;
        private Dictionary<string, string> _lookuptable;

        public ConstantDB()
        {
            _db = new Dictionary<string, IEnumerable<Constant>>();
            _lookuptable = new Dictionary<string, string>();
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

        public bool CanServeConstant(string name)
        {
            return _lookuptable.Keys.Contains(name);
        }

        public IEnumerable<Constant> GetCategory(string category)
        {
            return _db[category];
        }

        public Constant Lookup(string name)
        {
            var category = _lookuptable[name];
            return _db[category].Where(constant => constant.Name == name).FirstOrDefault();
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
                FillLookupTable(instance.Constants, instance.Category);
            }
        }

        private void FillLookupTable(IEnumerable<Constant> constants, string category)
        {
            foreach (var constant in constants)
            {
                _lookuptable.Add(constant.Name, category);
            }
        }
    }
}
