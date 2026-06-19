using System;
using ToyShop.Core.Interfaces;
using ToyShop.Infrastructure;
using UnityEngine;
using Zenject;

namespace ToyShop.UI.PauseMenu
{
    public class PauseMenuPresenter : IInitializable, IDisposable
    {
        private readonly IPauseService _pauseService;
        private readonly ISaveService _saveService;
        private readonly IInputProvider _inputProvider;
        private readonly ITabletStateService _tabletState;
        private readonly ISceneLoader _sceneLoader;
        private readonly PauseMenuView _view;

        public PauseMenuPresenter(
            IPauseService pauseService,
            ISaveService saveService,
            IInputProvider inputProvider,
            ITabletStateService tabletState,
            ISceneLoader sceneLoader,
            PauseMenuView view)
        {
            _pauseService = pauseService;
            _saveService = saveService;
            _inputProvider = inputProvider;
            _tabletState = tabletState;
            _sceneLoader = sceneLoader;
            _view = view;
        }

        public void Initialize()
        {
            _inputProvider.OnPausePressed += HandleEscapePressed;
            _pauseService.OnPauseStateChanged += HandlePauseStateChanged;

            _view.OnResumeClicked += HandleResumeClicked;
            _view.OnSaveClicked += HandleSaveClicked;
            _view.OnLoadClicked += HandleLoadClicked;
            _view.OnMainMenuClicked += HandleMainMenuClicked;
        }

        public void Dispose()
        {
            _inputProvider.OnPausePressed -= HandleEscapePressed;
            _pauseService.OnPauseStateChanged -= HandlePauseStateChanged;

            _view.OnResumeClicked -= HandleResumeClicked;
            _view.OnSaveClicked -= HandleSaveClicked;
            _view.OnLoadClicked -= HandleLoadClicked;
            _view.OnMainMenuClicked -= HandleMainMenuClicked;
        }

        private void HandleEscapePressed()
        {
            if (_tabletState.IsTabletOpen)
            {
                _tabletState.Close();
                return;
            }

            if (_pauseService.IsPaused) _pauseService.Resume();
            else _pauseService.Pause();
        }

        private void HandlePauseStateChanged(bool isPaused)
        {
            if (isPaused) _view.Show();
            else _view.Hide();
        }

        private void HandleResumeClicked() => _pauseService.Resume();

        private void HandleSaveClicked()
        {
            _saveService.Save();
            _view.ShowNotification("Game saved!", Color.green);
        }

        private void HandleLoadClicked()
        {
            if (!_saveService.HasSave)
            {
                _view.ShowNotification("No save found!", Color.red);
                return;
            }

            _saveService.Load();
            _view.ShowNotification("Game loaded!", Color.cyan);
        }

        private void HandleMainMenuClicked()
        {
            // Resume resets Time.timeScale — SceneLoader also resets as safety net
            _pauseService.Resume();
            _sceneLoader.LoadScene(SceneLoader.MainMenuScene);
        }
    }
}