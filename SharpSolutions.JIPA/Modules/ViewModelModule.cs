using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Modules
{
    public class ViewModelModule: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            Assembly asm = this.GetAssembly();

            builder.RegisterAssemblyTypes(asm).Where(t => t.Name.EndsWith("ViewModel")).AsSelf();
        }
    }
}
