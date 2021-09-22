using System;
using Cysharp.Threading.Tasks;
using Models.Abstracts;
using Plugins.Pool.Interfaces;
using UnityEngine;
using Zenject;
using IPoolable = Plugins.Pool.Interfaces.IPoolable;

namespace Models.Particles
{
    public class BombParticle : BaseParticle, IPoolable
    {
        [SerializeField] private ParticleSystem _bombParticle;
        
        async UniTaskVoid RemoveAfterSeconds(TimeSpan time)
        {
            await UniTask.Delay(time, ignoreTimeScale: false);
            gameObject.SetActive(false);
        }

        public IObjectPool Origin { get; set; }
        public void ReturnToPool()
        {
        }
        
        private void OnEnable()
        {
            _bombParticle.Play();
            RemoveAfterSeconds(TimeSpan.FromSeconds(1));
        }

        private void OnDisable()
        {
            Origin.ReturnToPool(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Boom collide with " + other.gameObject);
        }

        public class BombParticleFactory : PlaceholderFactory<BombParticle>
        {
        }
    }
}