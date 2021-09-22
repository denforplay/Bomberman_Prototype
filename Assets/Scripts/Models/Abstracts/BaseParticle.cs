using Plugins.Pool.Interfaces;
using UnityEngine;
using Zenject;
using IPoolable = Plugins.Pool.Interfaces.IPoolable;

namespace Models.Abstracts
{
    public class BaseParticle : MonoBehaviour, IPoolable
    {
        public IObjectPool Origin { get; set; }
        public void ReturnToPool()
        {
        }

    }
}