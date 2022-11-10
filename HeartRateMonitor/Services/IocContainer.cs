using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartRateMonitor.Services
{
    public static class IocContainer
    {
        private static readonly ConcurrentDictionary<Type, object> instances = new ConcurrentDictionary<Type, object>();

        public static void Register<T>() where T : class
            => instances[typeof(T)] = null;

        public static T Resolve<T>() where T : class
            => (T)GetInstance(typeof(T));

        private static object GetInstance(Type type)
        {
            bool registered = instances.TryGetValue(type, out object instance);
            if (instance == null)
            {
                object[] args = type.GetConstructors().First().GetParameters().Select(x => GetInstance(x.ParameterType)).ToArray();
                instance = Activator.CreateInstance(type, args);
                if (registered)
                    instances[type] = instance;
            }
            return instance;
        }
    }
}
