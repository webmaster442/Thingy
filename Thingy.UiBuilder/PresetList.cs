using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Thingy.FFMpegGui
{
    public class PresetList : List<Preset>
    {
        public PresetList()
        {
            var presettypes = from type in
                              Assembly.GetAssembly(typeof(Preset)).GetTypes()
                              where 
                              type.BaseType == typeof(Preset) &&
                              !type.IsAbstract && type.IsClass
                              select type;

            foreach (var type in presettypes)
            {
                var instance = Activator.CreateInstance(type) as Preset;
                Add(instance);
            }
        }
    }
}
