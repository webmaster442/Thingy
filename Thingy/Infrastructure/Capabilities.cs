using System.IO;
using System.Threading.Tasks;

namespace Thingy.Infrastructure
{
    public interface ICanImportExportXMLData
    {
        Task Import(Stream xmlData, bool append);
        Task Export(Stream xmlData);
    }
}
