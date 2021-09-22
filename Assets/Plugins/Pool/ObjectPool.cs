using System;
using System.Collections.Concurrent;
using Plugins.Pool.Interfaces;
using UnityEngine;

namespace Plugins.Pool
{
    public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private T _prefab;
        private readonly ConcurrentBag<T> _container = new ConcurrentBag<T>();
        private Func<T> _instantiateMethod;
        public ObjectPool(T prefab, Func<T> instantiateMethod)
        {
            _prefab = prefab;
            _instantiateMethod = instantiateMethod;
        }

        public T GetPrefabInstance()
        {
            T instance = null;
            if (_container.Count > 0 && _container.TryTake(out instance))
            {
                if (instance != null)
                {
                    instance.gameObject.SetActive(true);    
                }
            }
            else
            {
                instance = _instantiateMethod.Invoke();
            }
            instance.Origin = this;
            return instance;
        }

        public void ReturnToPool(T instance)
        {
            instance.gameObject.SetActive(false);
            instance.ReturnToPool();
            _container.Add(instance);
        }

        public void ReturnToPool(object instance)
        {
            if (instance is T)
            {
                ReturnToPool(instance as T);
            }
        }
    }
}
