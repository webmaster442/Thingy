using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Thingy.Modules
{
    public abstract class AbstractModule
    {
        public abstract string Name { get; }
        public abstract Uri IconPath { get; }
        public abstract UserControl RunModule();

        public override bool Equals(object obj)
        {
            return Equals(obj as AbstractModule);
        }

        public bool Equals(AbstractModule abstractModule)
        {
            if (abstractModule == null)
                return false;

            return Name == abstractModule.Name &&
                   IconPath == abstractModule.IconPath;
        }

        public override int GetHashCode()
        {
            var hashCode = -1820396915;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<Uri>.Default.GetHashCode(IconPath);
            return hashCode;
        }
    }
}
