using System;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.Items; 
using Zenject;
using UnityEngine;

namespace ToyShop.Gameplay.Services
{
    public class DeliveryService : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ICatalogService _catalog;
        private readonly IDeliveryPointProvider _deliveryPoint;
        private readonly BoxContainer.Factory _boxFactory;

        public DeliveryService(
            SignalBus signalBus,
            ICatalogService catalog,
            IDeliveryPointProvider deliveryPoint,
            BoxContainer.Factory boxFactory)
        {
            _signalBus = signalBus;
            _catalog = catalog;
            _deliveryPoint = deliveryPoint;
            _boxFactory = boxFactory;
        }

        public void Initialize() => _signalBus.Subscribe<PurchaseResultSignal>(OnPurchaseResult);

        public void Dispose() => _signalBus.Unsubscribe<PurchaseResultSignal>(OnPurchaseResult);

        private void OnPurchaseResult(PurchaseResultSignal signal)
        {
            if (!signal.Success) return;

            var purchasedToy = _catalog.GetToyById(signal.ToyId);
            if (purchasedToy == null) return;

          
            Vector3 spawnPos = _deliveryPoint.GetSpawnPosition();

  
            BoxContainer newBox = _boxFactory.Create();
            newBox.transform.position = spawnPos;

            
            newBox.SetupBox(purchasedToy);
        }
    }
}