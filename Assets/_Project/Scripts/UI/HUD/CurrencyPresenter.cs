using System;
using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.UI.HUD
{
    public class CurrencyPresenter : IInitializable, IDisposable
    {
        private readonly IEconomyService _economy;
        private readonly CurrencyView _view;

        private int _previousBalance;

        public CurrencyPresenter(IEconomyService economy, CurrencyView view)
        {
            _economy = economy;
            _view = view;
        }

        public void Initialize()
        {
            _previousBalance = _economy.CurrentBalance;
            _economy.OnBalanceChanged += HandleBalanceChanged;

            // Set initial display — delta 0 means no popup, no flash
            _view.UpdateBalance(_previousBalance, 0);
        }

        public void Dispose()
        {
            _economy.OnBalanceChanged -= HandleBalanceChanged;
        }

        private void HandleBalanceChanged(int newBalance)
        {
            int delta = newBalance - _previousBalance;
            _previousBalance = newBalance;
            _view.UpdateBalance(newBalance, delta);
        }
    }
}