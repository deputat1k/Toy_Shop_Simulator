using System;
using ToyShop.Core;
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
            _purchaseService.OnPurchaseCompleted += HandlePurchaseCompleted;
        public void Dispose() =>
            _purchaseService.OnPurchaseCompleted -= HandlePurchaseCompleted;

        private void HandlePurchaseCompleted(PurchaseResult result)
        {
            if (!result.Success) return;

            var toy = _catalog.GetToyById(result.ToyId);
            if (toy == null) return;

            BoxContainer box = _boxFactory.Create();
            box.transform.position = _deliveryPoint.GetSpawnPosition();
            box.SetupBox(toy);
        }
    }
}