using System;
using Zenject;
using ToyShop.Core.Interfaces;
using ToyShop.UI.HUD;

namespace ToyShop.UI.HUD
{
    public class HUDPresenter : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly HUDView _view;
        private readonly IEconomyService _economy;

        private int? _lastBalance = null;

        public HUDPresenter(SignalBus signalBus, HUDView view, IEconomyService economy)
        {
            _signalBus = signalBus;
            _view = view;
            _economy = economy;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<BalanceChangedSignal>(OnBalanceSignalReceived);
            _signalBus.Subscribe<TabletStateChangedSignal>(OnTabletStateChanged);

            UpdateBalanceView(_economy.CurrentBalance);
            _view.Show();
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<BalanceChangedSignal>(OnBalanceSignalReceived);
            _signalBus.Unsubscribe<TabletStateChangedSignal>(OnTabletStateChanged);
        }

        private void OnBalanceSignalReceived(BalanceChangedSignal signal)
        {
            UpdateBalanceView(signal.NewBalance);
        }

        private void OnTabletStateChanged(TabletStateChangedSignal signal)
        {
            if (_view == null) return;

            // Якщо планшет відкрито -> ховаємо HUD. Якщо закрито -> показуємо.
            if (signal.IsOpen) _view.Hide();
            else _view.Show();
        }

        private void UpdateBalanceView(int newBalance)
        {
            if (_view == null) return;
            if (_lastBalance == newBalance) return;

            _lastBalance = newBalance;
            _view.SetBalance(newBalance);
        }
    }
}