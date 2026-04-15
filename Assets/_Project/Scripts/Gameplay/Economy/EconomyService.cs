using ToyShop.Core.Interfaces;
using Zenject;

namespace ToyShop.Gameplay.Economy
{
    public class EconomyService : IEconomyService
    {
        private readonly SignalBus _signalBus;

        public int CurrentBalance { get; private set; } = 100;

        public EconomyService(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0) return false;

            if (CurrentBalance >= amount)
            {
                CurrentBalance -= amount;
                _signalBus.Fire(new BalanceChangedSignal(CurrentBalance));
                return true;
            }
            return false;
        }
    }
}