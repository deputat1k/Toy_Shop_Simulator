using System;
using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.Core.Controllers
{
    public class TabletStateService : ITabletStateService, IInitializable, IDisposable
    {
        private readonly IInputProvider _inputProvider;
        private readonly IPauseService _pauseService;

        public bool IsTabletOpen { get; private set; }
        public event Action<bool> OnTabletStateChanged;

        public TabletStateService(IInputProvider inputProvider, IPauseService pauseService)
        {
            _inputProvider = inputProvider;
            _pauseService = pauseService;
        }

        public void Initialize() =>
            _inputProvider.OnTabletTogglePressed += HandleTabletToggle;

        public void Dispose() =>
            _inputProvider.OnTabletTogglePressed -= HandleTabletToggle;

        public void Close()
        {
            if (!IsTabletOpen) return;
            IsTabletOpen = false;
            OnTabletStateChanged?.Invoke(false);
        }

        private void HandleTabletToggle()
        {
            // Prevent opening tablet while game is paused
            if (_pauseService.IsPaused) return;

            IsTabletOpen = !IsTabletOpen;
            OnTabletStateChanged?.Invoke(IsTabletOpen);
        }
    }
}