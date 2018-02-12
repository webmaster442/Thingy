using AppLib.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thingy.FFMpegGui;
using AppLib.Common.Extensions;

namespace Thingy.ViewModels
{
    public class FFMpegGuiViewModel: ViewModel
    {
        public PresetList Presets
        {
            get;
            private set;
        }

        public ObservableCollection<string> Files
        {
            get;
            private set;
        }

        public DelegateCommand AddFilesCommand { get; private set; }
        public DelegateCommand<string> RemoveSelectedCommand { get; private set; }
        public DelegateCommand ClearListCommand { get; private set; }

        public FFMpegGuiViewModel()
        {
            Presets = new PresetList();
            Files = new ObservableCollection<string>();
            AddFilesCommand = Command.ToCommand(AddFiles);
            RemoveSelectedCommand = Command.ToCommand<string>(RemoveSelected, CanRemove);
            ClearListCommand = Command.ToCommand(ClearList);
        }

        private void AddFiles()
        {
            var sfd = new System.Windows.Forms.OpenFileDialog();
            sfd.Multiselect = true;
            sfd.Filter = "All files|*.*";
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Files.AddRange(sfd.FileNames);
            }
        }

        private void RemoveSelected(string obj)
        {
            Files.Remove(obj);
        }

        private bool CanRemove(string obj)
        {
            return !string.IsNullOrEmpty(obj);
        }

        private void ClearList()
        {
            Files.Clear();
        }
    }
}
