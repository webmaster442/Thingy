using System;
using System.IO;
using System.Xml.Serialization;

namespace Thingy.Db.Entity
{
    public static class EntitySerializer
    {
        public static void Serialize<T>(Stream target, T input) where T: class
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (input == null)
                throw new ArgumentNullException(nameof(input));

            XmlSerializer xs = new XmlSerializer(typeof(T));
            xs.Serialize(target, input);
        }

        public static T Deserialize<T>(Stream source) where T: class
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            XmlSerializer xs = new XmlSerializer(typeof(T));

            return (T)xs.Deserialize(source);
        }
    }
}
