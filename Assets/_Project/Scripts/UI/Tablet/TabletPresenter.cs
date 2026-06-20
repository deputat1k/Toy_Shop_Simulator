using System;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using ToyShop.Gameplay.Cart;
using UnityEngine;
using Zenject;

namespace ToyShop.UI.Tablet
{
    public class TabletPresenter : IInitializable, IDisposable
    {
        private readonly ITabletStateService _tabletState;
        private readonly IPurchaseService _purchase;
        private readonly ICatalogService _catalog;
        private readonly ICartService _cart;
        private readonly IEconomyService _economy;
        private readonly TabletView _view;

        private bool _shopItemsGenerated;
        private bool _isCartTabActive;

        public TabletPresenter(
            ITabletStateService tabletState,
            IPurchaseService purchase,
            ICatalogService catalog,
            ICartService cart,
            IEconomyService economy,
            TabletView view)
        {
            _tabletState = tabletState;
            _purchase = purchase;
            _catalog = catalog;
            _cart = cart;
            _economy = economy;
            _view = view;
        }

        public void Initialize()
        {
            _tabletState.OnTabletStateChanged += HandleTabletStateChanged;
            _purchase.OnCartPurchased += HandleCartPurchased;
            _purchase.OnCartPurchaseFailed += HandleCartPurchaseFailed;
            _economy.OnBalanceChanged += HandleBalanceChanged;
            _cart.OnCartChanged += HandleCartChanged;

            _view.OnCloseClicked += HandleClose;
            _view.OnBuyAllClicked += HandleBuyAll;
            _view.OnShopTabClicked += HandleShopTabClicked;
            _view.OnCartTabClicked += HandleCartTabClicked;
        }

        public void Dispose()
        {
            _tabletState.OnTabletStateChanged -= HandleTabletStateChanged;
            _purchase.OnCartPurchased -= HandleCartPurchased;
            _purchase.OnCartPurchaseFailed -= HandleCartPurchaseFailed;
            _economy.OnBalanceChanged -= HandleBalanceChanged;
            _cart.OnCartChanged -= HandleCartChanged;

            _view.OnCloseClicked -= HandleClose;
            _view.OnBuyAllClicked -= HandleBuyAll;
            _view.OnShopTabClicked -= HandleShopTabClicked;
            _view.OnCartTabClicked -= HandleCartTabClicked;
        }

        private void HandleTabletStateChanged(bool isOpen)
        {
            if (!isOpen) { _view.Hide(); return; }

            if (!_shopItemsGenerated)
            {
                GenerateShopItems();
                _shopItemsGenerated = true;
            }

            _isCartTabActive = false;
            _view.UpdateBalance(_economy.CurrentBalance);
            _view.UpdateCartBadge(_cart.TotalItems);
            _view.ShowShopPage();
            _view.Show();
        }

        private void HandleClose() => _tabletState.Close();

        private void GenerateShopItems()
        {
            foreach (ToyData toy in _catalog.GetAllToys())
            {
                // Simple Instantiate — no Zenject Factory needed
                ShopItemView item = UnityEngine.Object.Instantiate(
    _view.ShopItemPrefab,
    _view.ShopItemsContainer);

                item.Setup(toy, _cart);
            }
        }

        private void HandleShopTabClicked()
        {
            _isCartTabActive = false;
            _view.ShowShopPage();
        }

        private void HandleCartTabClicked()
        {
            _isCartTabActive = true;
            RebuildCartView();
            _view.ShowCartPage();
        }

        private void HandleCartChanged()
        {
            _view.UpdateCartBadge(_cart.TotalItems);
            if (_isCartTabActive) RebuildCartView();
        }

        private void RebuildCartView()
        {
            foreach (Transform child in _view.CartItemsContainer)
                UnityEngine.Object.Destroy(child.gameObject);

            bool isEmpty = _cart.TotalItems == 0;
            _view.UpdateCartTotal(_cart.TotalPrice, isEmpty);
            if (isEmpty) return;

            foreach (CartItem cartItem in _cart.Items)
            {
                CartItemView itemView = UnityEngine.Object.Instantiate(
    _view.CartItemPrefab,
    _view.CartItemsContainer);

                itemView.Setup(cartItem, _cart);
            }
        }

        private void HandleBuyAll()
        {
            if (_cart.TotalItems == 0)
            {
                _view.ShowNotification("Cart is empty!", Color.yellow);
                return;
            }
            _purchase.TryBuyCart(_cart);
        }

        private void HandleCartPurchased(IReadOnlyList<string> toyIds)
        {
            string msg = toyIds.Count == 1
                ? "Order placed! 1 box incoming."
                : $"Order placed! {toyIds.Count} boxes incoming.";

            _view.ShowNotification(msg, Color.green);
            HandleShopTabClicked();
        }

        private void HandleCartPurchaseFailed() =>
            _view.ShowNotification("Not enough funds!", Color.red);

        private void HandleBalanceChanged(int newBalance)
        {
            if (_tabletState.IsTabletOpen)
                _view.UpdateBalance(newBalance);
        }
    }
}