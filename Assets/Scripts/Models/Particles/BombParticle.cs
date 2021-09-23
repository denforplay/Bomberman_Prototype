using System;
using Controllers;
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
        public IObjectPool Origin { get; set; }
        
        private async UniTaskVoid RemoveAfterSeconds(TimeSpan time)
        {
            await UniTask.Delay(time, ignoreTimeScale: false);
            gameObject.SetActive(false);
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
            if (other.gameObject.TryGetComponent(out Enemy enemy))
            {
                GameObject.Destroy(enemy.gameObject);
            }

            if (other.gameObject.TryGetComponent(out PigController pig))
            {
                pig.DecreaseSpeed(0.5f);
            }
            Debug.Log("Boom collide with " + other.gameObject);
        }
        
        public void ReturnToPool()
        {
        }

        public class BombParticleFactory : PlaceholderFactory<BombParticle>
        {
        }
    }
}