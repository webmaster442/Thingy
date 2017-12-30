using Thingy.Db;

namespace Thingy.Services
{
    public interface IService
    {
        void Configure(IApplication application, IDataBase db);
        void Job();
        long TriggerIntervalSeconds { get; set; }
    }
}
