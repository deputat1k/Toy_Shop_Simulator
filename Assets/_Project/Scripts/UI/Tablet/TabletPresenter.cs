using System;
using ToyShop.Core;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using UnityEngine;
using Zenject;

namespace ToyShop.UI.Tablet
{
    public class TabletPresenter : IInitializable, IDisposable
    {
        private readonly IGameStateService _gameState;
        private readonly IPurchaseService _purchase;
        private readonly ICatalogService _catalog;
        private readonly TabletView _view;
        private readonly ShopItemView.Factory _itemFactory;
        private bool _isInitialized;

        public TabletPresenter(
            IGameStateService gameState,
            IPurchaseService purchase,
            ICatalogService catalog,
            TabletView view,
            ShopItemView.Factory itemFactory)
        {
            _gameState = gameState;
            _purchase = purchase;
            _catalog = catalog;
            _view = view;
            _itemFactory = itemFactory;
        }

        public void Initialize()
        {
            _gameState.OnTabletStateChanged += HandleTabletStateChanged;
            _purchase.OnPurchaseCompleted += HandlePurchaseCompleted;
        }

        public void Dispose()
        {
            _gameState.OnTabletStateChanged -= HandleTabletStateChanged;
            _purchase.OnPurchaseCompleted -= HandlePurchaseCompleted;
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

        private void HandlePurchaseCompleted(PurchaseResult result)
        {
            if (result.Success) _view.ShowNotification("Successfully!", Color.green);
            else _view.ShowNotification("Not enough funds!", Color.red);
        }
    }
}