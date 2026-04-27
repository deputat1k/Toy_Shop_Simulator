using ToyShop.Core.Controllers;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using ToyShop.Gameplay.Economy;
using ToyShop.Gameplay.Environment;
using ToyShop.Gameplay.Factories;
using ToyShop.Gameplay.Items;
using ToyShop.Gameplay.Player;
using ToyShop.Gameplay.Services;
using ToyShop.Infrastructure;
using ToyShop.UI.HUD;
using ToyShop.UI.Tablet;
using UnityEngine;
using Zenject;

namespace ToyShop.Core.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Databases")]
        [SerializeField] private ToyDatabase _mainToyDatabase;

        [Header("UI Prefabs")]
        [SerializeField] private ShopItemView _shopItemPrefab;

        [Header("Item Prefabs")]
        [SerializeField] private BoxContainer _boxPrefab;

        public override void InstallBindings()
        {
            // INFRASTRUCTURE
            Container.BindInterfacesTo<DesktopInput>().AsSingle();
            Container.Bind<IInteractionScanner>().To<PhysicsRaycastScanner>().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();

            // DATA
            Container.BindInstance(_mainToyDatabase).AsSingle();

            // FACTORIES
            Container.Bind<ToyFactory>().AsSingle();
            Container.BindFactory<BoxContainer, BoxContainer.Factory>()
                     .FromComponentInNewPrefab(_boxPrefab)
                     .AsSingle();
            Container.BindFactory<Transform, ShopItemView, ShopItemView.Factory>()
                     .FromComponentInNewPrefab(_shopItemPrefab)
                     .AsSingle();

            // PLAYER
            Container.Bind<IPlayerController>().To<PlayerController>()
                     .FromComponentInHierarchy().AsSingle();

            // GAME STATE
            Container.BindInterfacesAndSelfTo<TabletStateService>().AsSingle(); // was GameStateService
            Container.BindInterfacesTo<CursorController>().AsSingle();
            Container.BindInterfacesTo<PlayerInputBlocker>().AsSingle();

            // SERVICES
            Container.BindInterfacesAndSelfTo<EconomyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CatalogService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PurchaseService>().AsSingle();

            // DELIVERY
            Container.BindInterfacesTo<DeliveryPoint>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<DeliveryService>().AsSingle().NonLazy();

            // UI (Currency)
            Container.Bind<CurrencyView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<CurrencyPresenter>().AsSingle().NonLazy();

            // UI (Tablet)
            Container.Bind<TabletView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<TabletPresenter>().AsSingle().NonLazy();
        }
    }
}