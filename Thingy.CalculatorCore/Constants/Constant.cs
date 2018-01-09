using System;
using System.Collections.Generic;

namespace Thingy.CalculatorCore.Constants
{
    public class Constant : IEquatable<Constant>
    {
        public string Description { get; private set; }
        public string Name { get; private set; }
        public double Value { get; private set; }

        internal Constant(string desc, string name, double val)
        {
            Description = desc;
            Name = name;
            Value = val;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Constant);
        }

        public bool Equals(Constant other)
        {
            return other != null &&
                   Description == other.Description &&
                   Name == other.Name &&
                   Value == other.Value;
        }

        public override int GetHashCode()
        {
            var hashCode = 535609977;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Constant constant1, Constant constant2)
        {
            return EqualityComparer<Constant>.Default.Equals(constant1, constant2);
        }

        public static bool operator !=(Constant constant1, Constant constant2)
        {
            return !(constant1 == constant2);
        }
    }
}
