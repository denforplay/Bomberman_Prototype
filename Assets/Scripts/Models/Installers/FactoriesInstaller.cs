using Models.Abstracts;
using Models.Particles;
using UnityEngine;
using Zenject;

namespace Models.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        [SerializeField] private Bomb _bomb;
        [SerializeField] private BombParticle _bombParticle;
        public override void InstallBindings()
        {
            Container.BindFactory<Bomb, Bomb.BombFactory>().FromComponentInNewPrefab(_bomb).AsSingle();
            Container.BindFactory<BombParticle, BombParticle.BombParticleFactory>().FromComponentInNewPrefab(_bombParticle).AsSingle();
        }
    }
}