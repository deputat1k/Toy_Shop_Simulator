using System;
using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Core.Controllers
{
    public class CursorController : IInitializable, IDisposable
    {
        private readonly ITabletStateService _gameState;

        public CursorController(ITabletStateService gameState) => _gameState = gameState;

        public void Initialize() =>
            _gameState.OnTabletStateChanged += HandleTabletStateChanged;
        public void Dispose() =>
            _gameState.OnTabletStateChanged -= HandleTabletStateChanged;

        private void HandleTabletStateChanged(bool isOpen)
        {
            Cursor.visible = isOpen;
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}