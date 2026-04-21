using System;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using Zenject;
using UnityEngine;

namespace ToyShop.UI.Tablet
{
    public class TabletPresenter : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly TabletView _view;
        private readonly ICatalogService _catalog;
        private readonly IPurchaseService _purchase;
        private readonly ShopItemView.Factory _itemFactory;
        private bool _isInitialized = false;

        public TabletPresenter(SignalBus signalBus, TabletView view, ICatalogService catalog, IPurchaseService purchase, ShopItemView.Factory itemFactory)
        {
            _signalBus = signalBus;
            _view = view;
            _catalog = catalog;
            _purchase = purchase;
            _itemFactory = itemFactory;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<TabletStateChangedSignal>(OnTabletStateChanged);
            _signalBus.Subscribe<PurchaseResultSignal>(OnPurchaseResult);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<TabletStateChangedSignal>(OnTabletStateChanged);
            _signalBus.Unsubscribe<PurchaseResultSignal>(OnPurchaseResult);
        }

        private void OnTabletStateChanged(TabletStateChangedSignal signal)
        {
            if (_view == null) return;

            if (signal.IsOpen)
            {
                if (!_isInitialized)
                {
                    ClearItemsContainer();
                    GenerateShopItems();
                    _isInitialized = true;
                }
                _view.Show();
            }
            else _view.Hide();
        }

        private void ClearItemsContainer()
        {
            foreach (Transform child in _view.ItemsContainer)
                GameObject.Destroy(child.gameObject);
        }

        private void GenerateShopItems()
        {
            var allToys = _catalog.GetAllToys();
            foreach (ToyData toy in allToys)
            {
                ShopItemView newItem = _itemFactory.Create(_view.ItemsContainer);
                newItem.Setup(toy, toyId => _purchase.TryBuyToy(toyId));
            }
        }

        private void OnPurchaseResult(PurchaseResultSignal signal)
        {
            if (_view == null) return;

            if (signal.Success) _view.ShowNotification("Successfully!", Color.green);
            else _view.ShowNotification("Not enough funds!", Color.red);
        }
    }
}