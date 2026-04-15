using ToyShop.Core.Interfaces;
using ToyShop.Data; // ВАЖЛИВО: Щоб бачило ToyDatabase
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
        // ВАЖЛИВО: Це поле потрібне, щоб прокинути базу даних з Unity
        [Header("Databases")]
        [SerializeField] private ToyDatabase _mainToyDatabase;

        public override void InstallBindings()
        {
            // --- 1. СИСТЕМНІ СЕРВІСИ ---
            Container.BindInterfacesAndSelfTo<DesktopInput>().AsSingle();
            Container.Bind<IInteractionScanner>().To<PhysicsRaycastScanner>().AsSingle();
            Container.Bind<Camera>().FromComponentInHierarchy().AsSingle();
            Container.Bind<ToyFactory>().AsSingle();

            // --- 2. СИГНАЛИ (Встановлюємо лише ОДИН раз) ---
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<InputTabletToggleSignal>();
            Container.DeclareSignal<TabletStateChangedSignal>();
            Container.DeclareSignal<BalanceChangedSignal>();
            Container.DeclareSignal<PurchaseResultSignal>();

            // --- 3. БАЗИ ДАНИХ ---
            Container.BindInstance(_mainToyDatabase).AsSingle();

            // --- 4. ГРАВЕЦЬ ---
            Container.Bind<IPlayerController>().To<PlayerController>().FromComponentInHierarchy().AsSingle();

            // --- 5. КОНТРОЛЕРИ СТАНУ ГРИ ---
            Container.BindInterfacesTo<GameStateController>().AsSingle();
            Container.BindInterfacesTo<CursorController>().AsSingle();
            Container.BindInterfacesTo<PlayerInputBlocker>().AsSingle();

            // --- 6. БІЗНЕС-ЛОГІКА ТА ЕКОНОМІКА (Один раз) ---
            Container.BindInterfacesTo<EconomyService>().AsSingle();
            Container.BindInterfacesTo<ToyShop.Gameplay.Services.CatalogService>().AsSingle().NonLazy();
            Container.BindInterfacesTo<ToyShop.Gameplay.Services.PurchaseService>().AsSingle();

            // --- 7. UI (HUD) ---
            Container.Bind<ToyShop.UI.HUD.HUDView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<ToyShop.UI.HUD.HUDPresenter>().AsSingle().NonLazy();
        }
    }
}