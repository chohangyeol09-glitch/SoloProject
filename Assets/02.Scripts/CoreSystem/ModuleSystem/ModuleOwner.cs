using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _02.Scripts.CoreSystem.ModuleSystem
{
    public class ModuleOwner : MonoBehaviour
    {
        protected Dictionary<Type, IModule> _moduleDict;

        private void Awake()
        {
            _moduleDict = GetComponentsInChildren<IModule>().ToDictionary(module => module.GetType());
            InitializeModules();
            AfterInitializeModules();
        }

        protected virtual void InitializeModules()
        {
            foreach (IModule module in _moduleDict.Values)
                module.Initialize(this);
        }

        protected virtual void AfterInitializeModules()
        {
            // if dict value have IAfterInit
            foreach (IAfterInitializeModule module in _moduleDict.Values.OfType<IAfterInitializeModule>())
                module.AfterInitialize();
        }

        public T GetModule<T>() where T : IModule
        {
            // return _moduleDict value
            if (_moduleDict.TryGetValue(typeof(T), out IModule module))
                return (T)module;
        
            
            IModule foundModule = _moduleDict.Values.FirstOrDefault(m => m is T);

            if (foundModule is T castedModule)
                return castedModule;
        
            return default(T);
        }

    }
}
