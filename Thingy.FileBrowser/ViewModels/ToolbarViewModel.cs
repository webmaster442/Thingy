using System.Collections.ObjectModel;
using System.Linq;
using Thingy.Db;

namespace Thingy.FileBrowser.ViewModels
{
    public class ToolbarViewModel
    {
        public ObservableCollection<string> ProgramNames { get; }

        public ToolbarViewModel(IDataBase db)
        {
            var programs = db.Programs.GetPrograms().Select(p => p.Name);
            ProgramNames = new ObservableCollection<string>(programs);
        }
    }
}
