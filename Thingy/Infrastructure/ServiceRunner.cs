using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Thingy.Db;
using Thingy.Services;

namespace Thingy.Infrastructure
{
    public sealed class ServiceRunner: IServiceRunner
    {
        private DispatcherTimer _timer;
        private List<IService> _services;
        private long _counter;

        public ServiceRunner()
        {
            _services = new List<IService>();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000),
                IsEnabled = true
            };
            _timer.Tick += _timer_Tick;
            _counter = 0;
            Configure();
        }

        public bool Enabled
        {
            get { return _timer.IsEnabled; }
            set { _timer.IsEnabled = value; }
        }

        private void Configure()
        {
            _services.Clear();
            var iservice = typeof(IService);

            var servicelist = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => iservice.IsAssignableFrom(type) &&
                       type.IsInterface == false &&
                       type.IsAbstract == false);

            foreach (var module in servicelist)
            {
                var instance = (IService)Activator.CreateInstance(module);
                instance.Configure(App.Instance, App.IoCContainer.Resolve<IDataBase>());
                _services.Add(instance);
            }
        }

        private async void _timer_Tick(object sender, EventArgs e)
        {
            ++_counter;
            foreach (var service in _services)
            {
                if (_counter % service.TriggerIntervalSeconds == 0)
                {
                    await Task.Run(() => service.Job());
                }
            }
        }
    }
}
