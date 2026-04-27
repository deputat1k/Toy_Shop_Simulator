using System;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.Items;
using Zenject;

namespace ToyShop.Gameplay.Services
{
    public class DeliveryService : IInitializable, IDisposable
    {
        private readonly IPurchaseService _purchaseService;
        private readonly ICatalogService _catalog;
        private readonly IDeliveryPointProvider _deliveryPoint;
        private readonly BoxContainer.Factory _boxFactory;

        public DeliveryService(
            IPurchaseService purchaseService,
            ICatalogService catalog,
            IDeliveryPointProvider deliveryPoint,
            BoxContainer.Factory boxFactory)
        {
            _purchaseService = purchaseService;
            _catalog = catalog;
            _deliveryPoint = deliveryPoint;
            _boxFactory = boxFactory;
        }

        public void Initialize() =>
            _purchaseService.OnPurchaseSucceeded += HandlePurchaseSucceeded;

        public void Dispose() =>
            _purchaseService.OnPurchaseSucceeded -= HandlePurchaseSucceeded;

        private void HandlePurchaseSucceeded(string toyId)
        {
            var toy = _catalog.GetToyById(toyId);
            if (toy == null) return;

            BoxContainer box = _boxFactory.Create();
            box.transform.position = _deliveryPoint.GetSpawnPosition();
            box.SetupBox(toy);
        }
    }
}