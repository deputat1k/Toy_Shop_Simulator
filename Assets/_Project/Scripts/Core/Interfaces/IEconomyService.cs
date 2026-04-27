using System;

namespace ToyShop.Core.Interfaces
{
    public interface IEconomyService
    {
        int CurrentBalance { get; }
        bool TrySpend(int amount);

        void Add(int amount);
        event Action<int> OnBalanceChanged;
    }
}