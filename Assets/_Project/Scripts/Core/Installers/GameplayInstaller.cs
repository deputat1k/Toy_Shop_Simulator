using ToyShop.Core.Interfaces;
using ToyShop.Data;
using ToyShop.Gameplay;
using ToyShop.Gameplay.Economy;
using ToyShop.Gameplay.Factories;
using ToyShop.Gameplay.Player;
using ToyShop.Infrastructure;
using UnityEngine;
using Zenject;

namespace ToyShop.Core.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        //  needed to dump the database from Unity
                [Header("Databases")]
        [SerializeField] private ToyDatabase _mainToyDatabase;

        [Header("UI Prefabs")]
        [SerializeField] private ToyShop.UI.Tablet.ShopItemView _shopItemPrefab;

        public override void InstallBindings()
        {
            // SYSTEM SERVICES
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<IInteractionScanner>().To<PhysicsRaycastScanner>().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ToyFactory>().AsSingle();

            //SIGNALS 
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<InputTabletToggleSignal>();
            Container.DeclareSignal<TabletStateChangedSignal>();
            Container.DeclareSignal<BalanceChangedSignal>();
            Container.DeclareSignal<PurchaseResultSignal>();

            // DATABASES
            Container.BindInstance(_mainToyDatabase).AsSingle();

            //PLAYER
            Container.Bind<IPlayerController>().To<PlayerController>().FromComponentInHierarchy().AsSingle();

            // GAME STATE CONTROLLERS
            Container.BindInterfacesTo<GameStateController>().AsSingle();
            Container.BindInterfacesTo<CursorController>().AsSingle();
            Container.BindInterfacesTo<PlayerInputBlocker>().AsSingle();

            //BUSINESS LOGIC AND ECONOMY
            Container.BindInterfacesTo<EconomyService>().AsSingle();
            Container.BindInterfacesTo<ToyShop.Gameplay.Services.CatalogService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ToyShop.Gameplay.Services.PurchaseService>().AsSingle();

            //UI (HUD)
            Container.Bind<ToyShop.UI.HUD.HUDView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<ToyShop.UI.HUD.HUDPresenter>().AsSingle().NonLazy();

            //UI (Tablet)
            Container.Bind<ToyShop.UI.Tablet.TabletView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<ToyShop.UI.Tablet.TabletPresenter>().AsSingle().NonLazy();

            Container.BindFactory<Transform, ToyShop.UI.Tablet.ShopItemView, ToyShop.UI.Tablet.ShopItemView.Factory>()
                     .FromComponentInNewPrefab(_shopItemPrefab)
                     .AsSingle();
        }
    }
}