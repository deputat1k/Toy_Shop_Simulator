using System;
using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.Core.Controllers
{
    public class PlayerInputBlocker : IInitializable, IDisposable
    {
        private readonly IGameStateService _gameState;
        private readonly IPlayerController _player;

        public PlayerInputBlocker(IGameStateService gameState, IPlayerController player)
        {
            _gameState = gameState;
            _player = player;
        }

        public void Initialize() =>
            _gameState.OnTabletStateChanged += HandleTabletStateChanged;
        public void Dispose() =>
            _gameState.OnTabletStateChanged -= HandleTabletStateChanged;

        private void HandleTabletStateChanged(bool isOpen)
        {
            if (isOpen) _player.DisableInput();
            else _player.EnableInput();
        }
    }
}