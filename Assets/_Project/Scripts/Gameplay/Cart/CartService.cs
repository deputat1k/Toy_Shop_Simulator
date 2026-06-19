using System;
using System.Collections.Generic;
using System.Linq;
using ToyShop.Core.Interfaces;
using ToyShop.Data;

namespace ToyShop.Gameplay.Cart
{
    public class CartService : ICartService
    {
        // Dictionary for O(1) lookup by toyId
        private readonly Dictionary<string, CartItem> _items =
            new Dictionary<string, CartItem>();

        public IReadOnlyList<CartItem> Items => _items.Values.ToList();

        public int TotalPrice => _items.Values.Sum(item => item.LineTotal);
        public int TotalItems => _items.Values.Sum(item => item.Quantity);

        public event Action OnCartChanged;

        public void AddItem(ToyData toy)
        {
            if (toy == null) return;

            if (_items.TryGetValue(toy.Id, out CartItem existing))
            {
                existing.Quantity++;
            }
            else
            {
                _items[toy.Id] = new CartItem(toy, 1);
            }

            OnCartChanged?.Invoke();
        }

        public void RemoveItem(string toyId)
        {
            if (!_items.ContainsKey(toyId)) return;

            _items.Remove(toyId);
            OnCartChanged?.Invoke();
        }

        public void ChangeQuantity(string toyId, int delta)
        {
            if (!_items.TryGetValue(toyId, out CartItem item)) return;

            item.Quantity += delta;

            if (item.Quantity <= 0)
                _items.Remove(toyId);

            OnCartChanged?.Invoke();
        }

        public void Clear()
        {
            if (_items.Count == 0) return;

            _items.Clear();
            OnCartChanged?.Invoke();
        }

        public bool HasItem(string toyId) =>
            _items.ContainsKey(toyId);

        public CartItem GetItem(string toyId) =>
            _items.TryGetValue(toyId, out CartItem item) ? item : null;
    }
}