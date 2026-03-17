using UnityEngine;
using Zenject;
using ToyShop.Core.Interfaces;
using ToyShop.Infrastructure;

namespace ToyShop.Core.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            
            Container.Bind<IInputProvider>()
                .To<DesktopInput>()
                .AsSingle();

            Container.Bind<IInteractionScanner>()
                .To<PhysicsRaycastScanner>()
                .AsSingle();

           
            Container.Bind<Camera>()
                .FromComponentInHierarchy()
                .AsSingle();
        }
    }
}