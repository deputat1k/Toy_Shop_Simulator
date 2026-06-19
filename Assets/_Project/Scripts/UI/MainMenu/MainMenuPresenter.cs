using System;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay;
using ToyShop.Infrastructure;
using UnityEngine;
using Zenject;

namespace ToyShop.UI.MainMenu
{
    public class MainMenuPresenter : IInitializable, IDisposable
    {
        private readonly MainMenuView _view;
        private readonly ISceneLoader _sceneLoader;
        private readonly ISaveService _saveService;

        public MainMenuPresenter(
            MainMenuView view,
            ISceneLoader sceneLoader,
            ISaveService saveService)
        {
            _view = view;
            _sceneLoader = sceneLoader;
            _saveService = saveService;
        }

        public void Initialize()
        {
            _view.OnNewGameClicked += HandleNewGame;
            _view.OnLoadGameClicked += HandleLoadGame;
            _view.OnExitClicked += HandleExit;

            // Disable Load if no save file exists
            _view.SetLoadButtonInteractable(_saveService.HasSave);
        }

        public void Dispose()
        {
            _view.OnNewGameClicked -= HandleNewGame;
            _view.OnLoadGameClicked -= HandleLoadGame;
            _view.OnExitClicked -= HandleExit;
        }

        private void HandleNewGame() =>
            _sceneLoader.LoadScene(SceneLoader.GameplayScene);

        private void HandleLoadGame()
        {
            // Flag for GameStartupController to auto-load save after scene transition
            PlayerPrefs.SetInt(GameStartupController.AutoLoadKey, 1);
            _sceneLoader.LoadScene(SceneLoader.GameplayScene);
        }

        private void HandleExit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}