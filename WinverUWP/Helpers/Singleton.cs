using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinverUWP.Helpers
{
    public class Singleton
    {
        private static ConcurrentDictionary<Type, object> _instances = new ConcurrentDictionary<Type, object>();

        public static T GetInstance<T>(T defaultValue = null) where T : class
        {
            if (_instances.TryGetValue(typeof(T), out var instance))
            {
                return (T)instance;
            }
            else if (defaultValue == null)
            {
                throw new InvalidOperationException("There are no instances of the specified type.");
            }
            else
            {
                _instances.TryAdd(typeof(T), defaultValue);
                return defaultValue;
            }
        }
    }
}
