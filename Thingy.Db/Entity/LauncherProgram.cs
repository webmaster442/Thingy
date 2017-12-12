using AppLib.MVVM;
using System;
using System.Collections.Generic;

namespace Thingy.Db.Entity
{
    public class LauncherProgram: BindableBase, IEquatable<LauncherProgram>
    {
        private string _name;
        private string _path;
        private string _params;

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

        public string Params
        {
            get { return _params; }
            set { SetValue(ref _params, value); }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as LauncherProgram);
        }

        public bool Equals(LauncherProgram other)
        {
            return other != null &&
                   _name == other._name &&
                   _path == other._path &&
                   _params == other._params;
        }

        public override int GetHashCode()
        {
            var hashCode = -1567945570;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_path);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_params);
            return hashCode;
        }

        public static bool operator ==(LauncherProgram program1, LauncherProgram program2)
        {
            return EqualityComparer<LauncherProgram>.Default.Equals(program1, program2);
        }

        public static bool operator !=(LauncherProgram program1, LauncherProgram program2)
        {
            return !(program1 == program2);
        }
    }
}
