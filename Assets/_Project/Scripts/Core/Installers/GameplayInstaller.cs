using ToyShop.Core.Controllers;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using ToyShop.Gameplay;
using ToyShop.Gameplay.Economy;
using ToyShop.Gameplay.Environment;
using ToyShop.Gameplay.Factories;
using ToyShop.Gameplay.Items;
using ToyShop.Gameplay.NPC;
using ToyShop.Gameplay.NPC.Spawning;
using ToyShop.Gameplay.Pause;
using ToyShop.Gameplay.Player;
using ToyShop.Gameplay.SaveSystem;
using ToyShop.Gameplay.Services;
using ToyShop.Infrastructure;
using ToyShop.UI.HUD;
using ToyShop.UI.PauseMenu;
using ToyShop.UI.Tablet;
using UnityEngine;
using Zenject;
using ToyShop.Gameplay.Cart;


namespace ToyShop.Core.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Databases")]
        [SerializeField] private ToyDatabase _mainToyDatabase;

        [Header("UI Prefabs")]
            

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
            

            // PLAYER
            Container.Bind<IPlayerController>().To<PlayerController>()
                     .FromComponentInHierarchy().AsSingle();

            // GAME STATE
            Container.BindInterfacesAndSelfTo<TabletStateService>().AsSingle();

            // PAUSE
            Container.BindInterfacesTo<PauseService>().AsSingle();

            // CURSOR & INPUT BLOCKING
            Container.BindInterfacesTo<CursorController>().AsSingle();
            Container.BindInterfacesTo<PlayerInputBlocker>().AsSingle();

            // SERVICES
            Container.BindInterfacesAndSelfTo<EconomyService>().AsSingle();
            Container.BindInterfacesAndSelfTo<CatalogService>().AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<PurchaseService>().AsSingle();
            Container.Bind<ICartService>().To<CartService>().AsSingle();

            // DELIVERY
            Container.BindInterfacesTo<DeliveryPoint>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<DeliveryService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<CartDeliveryController>()
         .FromComponentInHierarchy().AsSingle().NonLazy();

            // NPC SERVICES
            Container.BindInterfacesAndSelfTo<CheckoutService>().AsSingle();
            Container.Bind<ICheckoutQueue>()
                     .To<CheckoutCounter>()
                     .FromComponentInHierarchy()
                     .AsSingle();
            Container.Bind<IPointOfInterestProvider>()
                     .To<StorePointsOfInterest>()
                     .FromComponentInHierarchy()
                     .AsSingle();

            // NPC POOL & SPAWNER
            Container.BindInterfacesTo<NpcSpawner>()
                     .FromComponentInHierarchy()
                     .AsSingle()
                     .NonLazy();

            // SAVE
            Container.Bind<ISaveHandler>().To<EconomySaveHandler>().AsSingle();
            Container.Bind<ISaveHandler>().To<ShelfSaveHandler>().AsSingle();
            Container.Bind<ISaveHandler>().To<PlayerSaveHandler>().AsSingle();
            Container.Bind<ISaveHandler>().To<BoxSaveHandler>().AsSingle();
            Container.Bind<ISaveService>().To<SaveService>().AsSingle();

            // UI (HUD Notification)
            Container.Bind<HudNotificationView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IHudNotificationService>().To<HudNotificationService>().AsSingle();

            // UI (Currency)
            Container.Bind<CurrencyView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<CurrencyPresenter>().AsSingle().NonLazy();

            // UI (Tablet)
            Container.Bind<TabletView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<TabletPresenter>().AsSingle().NonLazy();

            // UI (Pause Menu)
            Container.Bind<PauseMenuView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<PauseMenuPresenter>().AsSingle().NonLazy();

            // SCENE LOADING
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

            // STARTUP (auto-load check on scene entry)
            Container.BindInterfacesTo<GameStartupController>().AsSingle().NonLazy();

            // HUD VISIBILITY
            Container.BindInterfacesTo<HudVisibilityController>().AsSingle().NonLazy();


        }

       
    }
}