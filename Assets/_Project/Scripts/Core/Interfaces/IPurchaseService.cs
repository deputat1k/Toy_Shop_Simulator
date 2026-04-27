using System;

namespace ToyShop.Core.Interfaces
{
    public interface IPurchaseService
    {
        bool TryBuyToy(string toyId);
        event Action<string> OnPurchaseSucceeded; // toyId
        event Action<string> OnPurchaseFailed;    // toyId
    }
}