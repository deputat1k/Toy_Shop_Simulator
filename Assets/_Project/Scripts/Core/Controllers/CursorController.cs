using System;
using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Core.Controllers
{
    public class CursorController : IInitializable, IDisposable
    {
        private readonly ITabletStateService _tabletState;
        private readonly IPauseService _pauseService;

        public CursorController(ITabletStateService tabletState, IPauseService pauseService)
        {
            _tabletState = tabletState;
            _pauseService = pauseService;
        }

        public void Initialize()
        {
            _tabletState.OnTabletStateChanged += HandleStateChanged;
            _pauseService.OnPauseStateChanged += HandleStateChanged;

            // Apply correct initial cursor state instead of relying on MouseLook.Start()
            UpdateCursor();
        }

        public void Dispose()
        {
            _tabletState.OnTabletStateChanged -= HandleStateChanged;
            _pauseService.OnPauseStateChanged -= HandleStateChanged;
        }

        private void HandleStateChanged(bool _) => UpdateCursor();

        private void UpdateCursor()
        {
            bool shouldShowCursor = _tabletState.IsTabletOpen || _pauseService.IsPaused;
            Cursor.visible = shouldShowCursor;
            Cursor.lockState = shouldShowCursor ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}