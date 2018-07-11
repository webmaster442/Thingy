using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Thingy.FFMpegGui
{
    public class PresetList : List<BasePreset>
    {
        public PresetList()
        {
            var presettypes = from type in
                              Assembly.GetAssembly(typeof(BasePreset)).GetTypes()
                              where 
                              (type.BaseType == typeof(BasePreset) ||
                              type.BaseType == typeof(BaseAudioPreset) ||
                              type.BaseType == typeof(BaseVideoPreset)) &&
                              !type.IsAbstract && type.IsClass
                              select type;

            foreach (var type in presettypes)
            {
                var instance = Activator.CreateInstance(type) as BasePreset;
                Add(instance);
            }
        }
    }
}
