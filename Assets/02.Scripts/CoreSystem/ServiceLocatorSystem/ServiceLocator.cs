using System;
using System.Collections.Generic;

namespace _02.Scripts.CoreSystem.ServiceLocatorSystem
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();

        public static void Register<T>(T service)
        {
            _services[typeof(T)] = service;
        }

        public static void UnRegister<T>()
        {
            _services.Remove(typeof(T));
        }

        public static T Get<T>()
        {
            if (_services.TryGetValue(typeof(T), out var service))
                return (T)service;
            
            return default;
        }
        
        
        
    }
}