using System.Windows;
using System.Windows.Threading;
using Thingy.API;

namespace Thingy.Implementation
{
    public class CmdHostProxy: ICmdHost
    {
        private Application _app;
        private object _lock;

        public CmdHostProxy(Application app)
        {
            _lock = new object();
            _app = app;
        }

        private ICmdHost RealImplementation
        {
            get
            {
                lock(_lock)
                {
                    return (_app.MainWindow as MainWindow)?.Terminal;
                }
            }
        }

        public string CurrentDirectory
        {
            get { return RealImplementation?.CurrentDirectory; }
            set
            {
                if (RealImplementation != null)
                {
                    RealImplementation.CurrentDirectory = value;
                }
            }
        }

        public Dispatcher HostDispatcher
        {
            get { return _app.Dispatcher; }
        }

        public void Clear()
        {
            RealImplementation?.Clear();
        }
    }
}
