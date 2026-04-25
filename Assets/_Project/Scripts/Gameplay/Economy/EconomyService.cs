using System;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Economy
{
    public class EconomyService : IEconomyService
    {
        public int CurrentBalance { get; private set; } = 100;

        public event Action<int> OnBalanceChanged;

        public bool TrySpend(int amount)
        {
            if (amount <= 0) return false;
            if (CurrentBalance < amount) return false;

            CurrentBalance -= amount;
            OnBalanceChanged?.Invoke(CurrentBalance);
            return true;
        }

        public void Add(int amount)
        {
            if (amount <= 0) return;

            CurrentBalance += amount;
            OnBalanceChanged?.Invoke(CurrentBalance);
        }
    }
}