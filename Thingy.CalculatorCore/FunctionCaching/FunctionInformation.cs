using System;
using System.Collections.Generic;

namespace Thingy.CalculatorCore.FunctionCaching
{
    /// <summary>
    /// Function Information.
    /// For quick replace purposes the ShortName acts like a key
    /// </summary>
    public class FunctionInformation : IEquatable<FunctionInformation>
    {
        public string FullName { get; }
        public List<string> Prototypes { get; }

        public FunctionInformation(string fullName, string defaultPrototype)
        {
            FullName = fullName;
            Prototypes = new List<string>
            {
                defaultPrototype
            };
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FunctionInformation);
        }

        public bool Equals(FunctionInformation other)
        {
            return other != null &&
                   FullName == other.FullName &&
                   EqualityComparer<List<string>>.Default.Equals(Prototypes, other.Prototypes);
        }

        public override int GetHashCode()
        {
            var hashCode = 1544853645;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullName);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(Prototypes);
            return hashCode;
        }

        public static bool operator ==(FunctionInformation information1, FunctionInformation information2)
        {
            return EqualityComparer<FunctionInformation>.Default.Equals(information1, information2);
        }

        public static bool operator !=(FunctionInformation information1, FunctionInformation information2)
        {
            return !(information1 == information2);
        }
    }
}
