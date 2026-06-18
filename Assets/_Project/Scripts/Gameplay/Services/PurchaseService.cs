using System;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using ToyShop.Gameplay.Cart;

namespace ToyShop.Gameplay.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IEconomyService _economy;
        private readonly ICatalogService _catalog;

        public event Action<string> OnPurchaseSucceeded;
        public event Action<string> OnPurchaseFailed;
        public event Action<IReadOnlyList<string>> OnCartPurchased;
        public event Action OnCartPurchaseFailed;

        public PurchaseService(IEconomyService economy, ICatalogService catalog)
        {
            _economy = economy;
            _catalog = catalog;
        }

        // Single item — immediate delivery via DeliveryService
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

        // Cart checkout — atomic deduction, sequential delivery
        public bool TryBuyCart(ICartService cart)
        {
            if (cart.TotalItems == 0) return false;

            // Single atomic spend — player either buys everything or nothing
            if (!_economy.TrySpend(cart.TotalPrice))
            {
                OnCartPurchaseFailed?.Invoke();
                return false;
            }

            // Expand cart items into ordered delivery list:
            // Toy1 × 2, Toy2 × 1  →  ["toy1", "toy1", "toy2"]
            var deliveryIds = new List<string>();
            foreach (CartItem item in cart.Items)
            {
                for (int i = 0; i < item.Quantity; i++)
                    deliveryIds.Add(item.ToyData.Id);
            }

            cart.Clear();
            OnCartPurchased?.Invoke(deliveryIds);
            return true;
        }
    }
}