using System;
using ToyShop.Core;
using ToyShop.Core.Interfaces;
using ToyShop.Data;

namespace ToyShop.Gameplay.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IEconomyService _economy;
        private readonly ICatalogService _catalog;

        public event Action<PurchaseResult> OnPurchaseCompleted;

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
                OnPurchaseCompleted?.Invoke(new PurchaseResult(toyId, false));
                return false;
            }

            bool success = _economy.TrySpend(toy.PurchasePrice);
            OnPurchaseCompleted?.Invoke(new PurchaseResult(toyId, success));
            return success;
        }
    }
}