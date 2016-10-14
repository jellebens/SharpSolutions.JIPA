using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Diagnostics;

namespace SharpSolutions.JIPA.Modules
{
    public class LoggingModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly asm = this.GetAssembly();

            builder.Register(l => new LoggingChannel(Configuration.Current.ClientId, null, new Guid("DE91B5A6-6B6F-44CB-A70E-02A3A636B01F"))).AsSelf();
        }
    }
}
