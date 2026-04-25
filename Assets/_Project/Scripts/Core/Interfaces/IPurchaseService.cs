using System;

namespace ToyShop.Core.Interfaces
{
    public interface IPurchaseService
    {
        bool TryBuyToy(string toyId);
        event Action<PurchaseResult> OnPurchaseCompleted;
    }
}