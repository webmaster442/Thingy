using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Thingy.CoreModules.Models
{
    public class SystemProgram : BindableBase, IEquatable<SystemProgram>
    {
        private string _name;
        private string _path;
        private ImageSource _icon;


        public string Name
        {
            get { return _name; }
            set { SetValue(ref _name, value); }
        }

        public string Path
        {
            get { return _path; }
            set { SetValue(ref _path, value); }
        }

        public ImageSource Icon
        {
            get { return _icon; }
            set { SetValue(ref _icon, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SystemProgram);
        }

        public bool Equals(SystemProgram other)
        {
            return other != null &&
                   _name == other._name &&
                   _path == other._path;
        }

        public override int GetHashCode()
        {
            var hashCode = -827305254;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_path);
            return hashCode;
        }

        public static bool operator ==(SystemProgram program1, SystemProgram program2)
        {
            return EqualityComparer<SystemProgram>.Default.Equals(program1, program2);
        }

        public static bool operator !=(SystemProgram program1, SystemProgram program2)
        {
            return !(program1 == program2);
        }
    }
}
