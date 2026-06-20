using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.SaveSystem;
using ToyShop.Infrastructure;
using ToyShop.UI.MainMenu;
using Zenject;

namespace ToyShop.Core.Installers
{
    public class MainMenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Scene loading
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

            // SaveService — MainMenu only needs HasSave check, not actual handlers
            // Zenject injects empty List<ISaveHandler> automatically when nothing is bound
            Container.Bind<ISaveService>().To<SaveService>().AsSingle();

            // UI
            Container.Bind<MainMenuView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<MainMenuPresenter>().AsSingle().NonLazy();
        }
    }
}