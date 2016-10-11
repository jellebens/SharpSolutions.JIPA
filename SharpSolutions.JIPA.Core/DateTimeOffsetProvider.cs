using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpSolutions.JIPA.Core
{
    public class DateTimeOffsetProvider
    {
        [ThreadStatic]
        private static Func<DateTimeOffset> provider;

        static DateTimeOffsetProvider() { }

        private static Func<DateTimeOffset> Provider
        {
            get { return provider ?? (provider = () => DateTime.Now); }
        }


        public static DateTimeOffset Now
        {
            get{ return Provider(); } 
        }

        public static IDisposable Override(DateTimeOffset pointInTime)
        {
            var value = Provider;
            var undo = new DisposableAction(() => provider = value);
            provider = () => pointInTime;
            return undo;
        }
    }
}
