using Managers;
using UnityEngine;
using Zenject;

namespace Models.Installers
{
    public class ManagersInstaller : MonoInstaller
    {
        [SerializeField] private ParticleManager _particles;
        public override void InstallBindings()
        {
            Container.BindInstance(_particles);
        }
    }
}