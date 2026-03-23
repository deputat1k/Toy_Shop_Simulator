using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.Factories;
using ToyShop.Infrastructure;
using UnityEngine;
using Zenject;

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

            Container.Bind<ToyFactory>()
                .AsSingle();
        }
    }
}