using System;
using ToyShop.Core.Interfaces;
using ToyShop.UI.HUD;
using Zenject;

namespace ToyShop.Gameplay
{
    // Hides the HUD balance panel when tablet is open
    // The tablet shows balance in its own header — no need for both visible
    public class HudVisibilityController : IInitializable, IDisposable
    {
        private readonly ITabletStateService _tabletState;
        private readonly CurrencyView _currencyView;

        public HudVisibilityController(
            ITabletStateService tabletState,
            CurrencyView currencyView)
        {
            _tabletState = tabletState;
            _currencyView = currencyView;
        }

        public void Initialize() =>
            _tabletState.OnTabletStateChanged += HandleTabletStateChanged;

        public void Dispose() =>
            _tabletState.OnTabletStateChanged -= HandleTabletStateChanged;

        private void HandleTabletStateChanged(bool isTabletOpen)
        {
            if (_currencyView == null) return;

            // Show HUD balance only when tablet is closed
            _currencyView.gameObject.SetActive(!isTabletOpen);
        }
    }
}