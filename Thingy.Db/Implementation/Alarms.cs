using LiteDB;
using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db.Implementation
{
    internal class Alarms: ImplementationBase<Alarm>, IAlarms 
    {
        public Alarms(LiteCollection<Alarm> collection) : base(collection)
        {
        }

        public IEnumerable<Alarm> GetAlarms()
        {
            return EntityCollection.FindAll();
        }

        public IEnumerable<Alarm> GetActiveAlarms()
        {
            return EntityCollection.Find(alarm => alarm.Active == true);
        }
    }
}
