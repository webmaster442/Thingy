using IronPython.Runtime.Types;
using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Thingy.CalculatorCore.FunctionCaching
{
    public static class FunctionCache
    {
        private static void AddMethodsToFunctionCache(ref Dictionary<string, FunctionInformation> cache, Type type)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Static);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) continue;
                var name = method.Name;
                var fullName = string.Format("{0}.{1}", method.ReflectedType.Name, name);

                if (cache.ContainsKey(name))
                {
                    cache[name].Prototypes.Add(GetMethodPrototype(method));
                }
                else
                {
                    cache.Add(name, new FunctionInformation(fullName, GetMethodPrototype(method)));
                }
            }
        }

        private static string GetMethodPrototype(MethodInfo method)
        {
            StringBuilder value = new StringBuilder();

            value.AppendFormat("{0} {1}(", method.ReturnType.Name, method.Name);
            var parameters = method.GetParameters();
            for (int i = 0; i < parameters.Length; i++)
            {
                var parmeterInfo = parameters[i];
                value.AppendFormat("{0} {1}", parmeterInfo.ParameterType.Name, parmeterInfo.Name);
                if (i != parameters.Length - 1)
                {
                    value.Append(", ");
                }
            }
            value.Append(")");
            return value.ToString();
        }

        public static void Fill(ref Dictionary<string, FunctionInformation> cache, ref ScriptScope scope, params Type[] types)
        {
            foreach (var type in types)
            {
                scope.SetVariable(type.Name, DynamicHelpers.GetPythonTypeFromType(type));
                AddMethodsToFunctionCache(ref cache, type);
            }
        }
    }
}
