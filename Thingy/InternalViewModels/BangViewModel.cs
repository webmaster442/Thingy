using AppLib.MVVM;
using Thingy.API;
using Thingy.Implementation.Bang;
using Thingy.InternalModules;

namespace Thingy.InternalViewModels
{
    internal class BangViewModel: ViewModel<IBangView>
    {
        private readonly IApplication _app;
        private string _BangSearchText;

        public BangViewModel(IBangView view, IApplication app): base(view)
        {
            _app = app;
            BangProvider = new BangProvider();
            CancelCommand = Command.CreateCommand(Cancel);
            OkCommand = Command.CreateCommand(Ok);
            SearchCommand = Command.CreateCommand(Search);
        }

        public DelegateCommand CancelCommand { get; }
        public DelegateCommand OkCommand { get; }
        public DelegateCommand SearchCommand { get; }

        public string BangSearchText
        {
            get { return _BangSearchText; }
            set { SetValue(ref _BangSearchText, value); }
        }

        public BangProvider BangProvider
        {
            get;
        }

        private void Search()
        {

        }

        private void Ok()
        {
            View.Close();
        }

        private void Cancel()
        {
            View.Close();
        }
    }
}
