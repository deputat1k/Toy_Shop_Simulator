using System;
using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.Core.Controllers
{
    public class TabletStateService : ITabletStateService, IInitializable, IDisposable
    {
        private readonly IInputProvider _inputProvider;

        public bool IsTabletOpen { get; private set; }

        public event Action<bool> OnTabletStateChanged;

        public TabletStateService(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        public void Initialize() =>
            _inputProvider.OnTabletTogglePressed += HandleTabletToggle;

        public void Dispose() =>
            _inputProvider.OnTabletTogglePressed -= HandleTabletToggle;

        private void HandleTabletToggle()
        {
            IsTabletOpen = !IsTabletOpen;
            OnTabletStateChanged?.Invoke(IsTabletOpen);
        }
    }
}