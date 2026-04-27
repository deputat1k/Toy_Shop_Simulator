using System;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using UnityEngine;
using Zenject;

namespace ToyShop.UI.Tablet
{
    public class TabletPresenter : IInitializable, IDisposable
    {
        private readonly ITabletStateService _tabletState;
        private readonly IPurchaseService _purchase;
        private readonly ICatalogService _catalog;
        private readonly TabletView _view;
        private readonly ShopItemView.Factory _itemFactory;
        private bool _isInitialized;

        public TabletPresenter(
            ITabletStateService tabletState,
            IPurchaseService purchase,
            ICatalogService catalog,
            TabletView view,
            ShopItemView.Factory itemFactory)
        {
            _tabletState = tabletState;
            _purchase = purchase;
            _catalog = catalog;
            _view = view;
            _itemFactory = itemFactory;
        }

        public void Initialize()
        {
            _tabletState.OnTabletStateChanged += HandleTabletStateChanged;
            _purchase.OnPurchaseSucceeded += HandlePurchaseSucceeded;
            _purchase.OnPurchaseFailed += HandlePurchaseFailed;
        }

        public void Dispose()
        {
            _tabletState.OnTabletStateChanged -= HandleTabletStateChanged;
            _purchase.OnPurchaseSucceeded -= HandlePurchaseSucceeded;
            _purchase.OnPurchaseFailed -= HandlePurchaseFailed;
        }

        private void HandleTabletStateChanged(bool isOpen)
        {
            if (!isOpen)
            {
                _view.Hide();
                return;
            }

            if (!_isInitialized)
            {
                ClearItemsContainer();
                GenerateShopItems();
                _isInitialized = true;
            }

            _view.Show();
        }

        private void ClearItemsContainer()
        {
            foreach (Transform child in _view.ItemsContainer)
                UnityEngine.Object.Destroy(child.gameObject);
        }

        private void GenerateShopItems()
        {
            foreach (ToyData toy in _catalog.GetAllToys())
            {
                ShopItemView item = _itemFactory.Create(_view.ItemsContainer);
                item.Setup(toy, toyId => _purchase.TryBuyToy(toyId));
            }
        }

        private void HandlePurchaseSucceeded(string toyId) =>
            _view.ShowNotification("Successfully!", Color.green);

        private void HandlePurchaseFailed(string toyId) =>
            _view.ShowNotification("Not enough funds!", Color.red);
    }
}