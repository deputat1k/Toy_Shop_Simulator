using System;
using ToyShop.Core.Interfaces;
using ToyShop.Data;

namespace ToyShop.Gameplay.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IEconomyService _economy;
        private readonly ICatalogService _catalog;

        public event Action<string> OnPurchaseSucceeded;
        public event Action<string> OnPurchaseFailed;

        public PurchaseService(IEconomyService economy, ICatalogService catalog)
        {
            _economy = economy;
            _catalog = catalog;
        }

        public bool TryBuyToy(string toyId)
        {
            ToyData toy = _catalog.GetToyById(toyId);
            if (toy == null)
            {
                OnPurchaseFailed?.Invoke(toyId);
                return false;
            }

            if (!_economy.TrySpend(toy.PurchasePrice))
            {
                OnPurchaseFailed?.Invoke(toyId);
                return false;
            }

            OnPurchaseSucceeded?.Invoke(toyId);
            return true;
        }
    }
}