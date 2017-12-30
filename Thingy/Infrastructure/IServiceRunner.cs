using Thingy.Services;

namespace Thingy.Infrastructure
{
    interface IServiceRunner
    {
        bool Enabled { get; set; }
    }
}
