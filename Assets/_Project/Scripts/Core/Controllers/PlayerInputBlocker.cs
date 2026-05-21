using System;
using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.Core.Controllers
{
    public class PlayerInputBlocker : IInitializable, IDisposable
    {
        private readonly ITabletStateService _tabletState;
        private readonly IPauseService _pauseService;
        private readonly IPlayerController _player;

        public PlayerInputBlocker(
            ITabletStateService tabletState,
            IPauseService pauseService,
            IPlayerController player)
        {
            _tabletState = tabletState;
            _pauseService = pauseService;
            _player = player;
        }

        public void Initialize()
        {
            _tabletState.OnTabletStateChanged += HandleStateChanged;
            _pauseService.OnPauseStateChanged += HandleStateChanged;
        }

        public void Dispose()
        {
            _tabletState.OnTabletStateChanged -= HandleStateChanged;
            _pauseService.OnPauseStateChanged -= HandleStateChanged;
        }

        // Input is blocked if ANY blocking state is active
        private void HandleStateChanged(bool _)
        {
            bool shouldBlock = _tabletState.IsTabletOpen || _pauseService.IsPaused;
            if (shouldBlock) _player.DisableInput();
            else _player.EnableInput();
        }
    }
}