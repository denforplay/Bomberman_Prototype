using System;
using System.Collections.Generic;
using Models.Abstracts;
using Models.Particles;
using Plugins.Pool;
using Plugins.Pool.Interfaces;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class ParticleManager : MonoBehaviour
    {
        [SerializeField]
        private List<BaseParticle> _prefabs;

        [SerializeField]
        private GameObject _particleContainer;
        private Dictionary<Type, IObjectPool<BaseParticle>> _particlePools;
        private BombParticle.BombParticleFactory _bombParticleFactory;

        [Inject]
        public void Init(BombParticle.BombParticleFactory bombParticleFactory)
        {
            _bombParticleFactory = bombParticleFactory;
        }
        
        private void Start()
        {
            _particlePools = new Dictionary<Type, IObjectPool<BaseParticle>>();

            foreach (var prefab in _prefabs)
            {
                _particlePools[prefab.GetType()] =
                    new ObjectPool<BaseParticle>(prefab, 
                        () =>
                        {
                            if (prefab.GetType() == typeof(BombParticle))
                            {
                                var particle = _bombParticleFactory.Create();
                                particle.transform.SetParent(_particleContainer.transform);
                                return particle;
                            }
                            else
                            {
                                throw new ArgumentException("No factory for this object");
                            }
                        });
            }
        }

        public BaseParticle GetParticle(Type particleType)
        {
            if (_particlePools[particleType] != null)
            {
                return _particlePools[particleType].GetPrefabInstance();
            }
            else
            {
                throw new ArgumentException("No such particle");
            }
        }
    }
}