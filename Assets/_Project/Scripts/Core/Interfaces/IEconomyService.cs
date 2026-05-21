using System;

namespace ToyShop.Core.Interfaces
{
    public interface IEconomyService
    {
        int CurrentBalance { get; }
        bool TrySpend(int amount);
        void Add(int amount);

        // Required for save/load — directly restores balance without side effects
        void SetBalance(int amount);

        event Action<int> OnBalanceChanged;
    }
}