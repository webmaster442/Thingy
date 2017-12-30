using System.Collections.Generic;
using Thingy.Db.Entity;

namespace Thingy.Db
{
    public interface IAlarms
    {
        IEnumerable<Alarm> GetAlarms();
        IEnumerable<Alarm> GetActiveAlarms();
    }
}
