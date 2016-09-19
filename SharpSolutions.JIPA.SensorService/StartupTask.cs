using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using System.Diagnostics;
using SharpSolutions.JIPA.Sensors;
using Windows.System.Threading;
using Autofac;
using System.Reflection;
using SharpSolutions.JIPA.SensorService.Modules;
using SharpSolutions.JIPA.SensorService.Services;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace SharpSolutions.JIPA.SensorService
{
    public sealed class StartupTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _Deferral;
        private IContainer _Container;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _Deferral = taskInstance.GetDeferral();

            Initialize();

            IEnumerable<IService> services = _Container.Resolve<IEnumerable<IService>>();

            foreach (IService svc in services)
            {
                await svc.Start();
            }
        }

        private void Initialize() {
            ContainerBuilder builder = new ContainerBuilder();

            Assembly asm = typeof(IService).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(asm).Where(t => t.IsAssignableTo<IService>()).As<IService>();

            _Container = builder.Build();

        }

        
    }
}
