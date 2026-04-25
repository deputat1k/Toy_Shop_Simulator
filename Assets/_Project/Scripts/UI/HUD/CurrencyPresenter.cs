using System;
using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.UI.HUD
{
    public class CurrencyPresenter : IInitializable, IDisposable
    {
        private readonly IEconomyService _economy;
        private readonly IGameStateService _gameState;
        private readonly CurrencyView _view;

        public CurrencyPresenter(IEconomyService economy, IGameStateService gameState, CurrencyView view)
        {
            _economy = economy;
            _gameState = gameState;
            _view = view;
        }

        public void Initialize()
        {
            _economy.OnBalanceChanged += HandleBalanceChanged;
            _gameState.OnTabletStateChanged += HandleTabletStateChanged;

            _view.SetBalance(_economy.CurrentBalance);
            _view.Show();
        }

        public void Dispose()
        {
            _economy.OnBalanceChanged -= HandleBalanceChanged;
            _gameState.OnTabletStateChanged -= HandleTabletStateChanged;
        }

        private void HandleBalanceChanged(int newBalance) => _view.SetBalance(newBalance);

        private void HandleTabletStateChanged(bool isOpen)
        {
            if (isOpen) _view.Hide();
            else _view.Show();
        }
    }
}