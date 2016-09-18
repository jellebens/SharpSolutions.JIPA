using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using System.Reflection;

namespace SharpSolutions.JIPA.Modules
{
    public class ServiceModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly asm = this.GetAssembly();

            builder.RegisterAssemblyTypes(asm).Where(t => t.Name.EndsWith("Service"));
        }
    }
}
