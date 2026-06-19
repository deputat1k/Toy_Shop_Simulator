using System;
using System.Collections.Generic;
using ToyShop.Data;
using ToyShop.Gameplay.Cart;

namespace ToyShop.Core.Interfaces
{
    public interface ICartService
    {
        // Read-only snapshot — changes reflected via OnCartChanged
        IReadOnlyList<CartItem> Items { get; }

        int TotalPrice { get; }
        int TotalItems { get; }

        void AddItem(ToyData toy);
        void RemoveItem(string toyId);

        // delta: +1 or -1; removes item automatically when quantity reaches 0
        void ChangeQuantity(string toyId, int delta);

        void Clear();

        bool HasItem(string toyId);
        CartItem GetItem(string toyId);

        event Action OnCartChanged;
    }
}