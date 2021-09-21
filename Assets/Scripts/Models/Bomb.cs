using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Models
{
    public class Bomb : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _bombParticle;
        [SerializeField] private BoxCollider2D _collider;
        private void OnTriggerExit2D(Collider2D other)
        {
            _collider.isTrigger = false;
        }
        
        private void OnDisable()
        {
            Instantiate(_bombParticle).transform.position = transform.position;
        }
    }
}