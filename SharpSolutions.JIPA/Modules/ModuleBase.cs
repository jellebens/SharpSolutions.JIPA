using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Modules
{
    public static class ModuleExentions
    {
        public static Assembly GetAssembly(this Autofac.Module module) {
            return module.GetType().GetTypeInfo().Assembly;
        }
    }
}
