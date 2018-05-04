using AppLib.MVVM;

namespace Thingy.Mpv.ViewModels
{
    public class MpvViewModel: BindableBase
    {
        private string _Arguments;

        public string Arguments
        {
            get { return _Arguments; }
            set { SetValue(ref _Arguments, value); }
        }

        public DelegateCommand<string> AddArgumentCommand { get; }
        public DelegateCommand<string> DeleteArgumentCommand { get; }

        public MpvViewModel()
        {
            AddArgumentCommand = Command.ToCommand<string>(AddArgument, CanAddArgument);
            DeleteArgumentCommand = Command.ToCommand<string>(DeleteArgument, CanDeleteArgument);
        }

        private void AddArgument(string obj)
        {
            _Arguments += obj;
        }

        private bool CanAddArgument(string obj)
        {
            return !_Arguments.Contains(obj);
        }

        private void DeleteArgument(string obj)
        {
            _Arguments = _Arguments.Replace(obj, "");
        }

        private bool CanDeleteArgument(string obj)
        {
            return _Arguments.Contains(obj);
        }
    }
}
