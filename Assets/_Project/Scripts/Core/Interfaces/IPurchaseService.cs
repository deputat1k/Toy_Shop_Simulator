using System;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;

namespace ToyShop.Core.Interfaces
{
    public interface IPurchaseService
    {
        // Single item purchase — used by old flow, kept for compatibility
        bool TryBuyToy(string toyId);

        // Cart checkout — atomic: all money deducted at once or nothing
        bool TryBuyCart(ICartService cart);

        event Action<string> OnPurchaseSucceeded;
        event Action<string> OnPurchaseFailed;

        // Fires with ordered delivery list after successful cart purchase
        event Action<IReadOnlyList<string>> OnCartPurchased;
        event Action OnCartPurchaseFailed;
    }
}