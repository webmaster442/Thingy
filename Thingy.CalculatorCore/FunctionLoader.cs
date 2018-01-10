using IronPython.Runtime.Types;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Thingy.CalculatorCore
{
    public class FunctionLoader
    {
        private ScriptScope _scope;
        private Dictionary<string, string> _functioncache;

        public FunctionLoader(ScriptScope scope, Dictionary<string, string> functionCache)
        {
            _scope = scope;
            _functioncache = functionCache;
        }

        public void LoadTypesToScope(params Type[] types)
        {
            foreach (var type in types)
            {
                _scope.SetVariable(type.Name, DynamicHelpers.GetPythonTypeFromType(type));
                AddMethodsToFunctionCache(type);
            }
        }

        private void AddMethodsToFunctionCache(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) continue;
                var FullName = string.Format("{0}.{1}", method.ReflectedType.Name, method.Name);
                var Name = method.Name;

                if (_functioncache.ContainsKey(Name)) continue;
                _functioncache.Add(Name, FullName);
            }
        }
    }
}
