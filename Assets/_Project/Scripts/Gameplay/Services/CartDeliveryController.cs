using System;
using System.Collections;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.Items;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.Services
{
    // MonoBehaviour required for coroutine-based sequential spawning
    // Place on any active scene GameObject (e.g. NpcSpawner or its own empty GO)
    public class CartDeliveryController : MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private float _spawnInterval = 0.8f;

        private IPurchaseService _purchaseService;
        private ICatalogService _catalog;
        private IDeliveryPointProvider _deliveryPoint;
        private BoxContainer.Factory _boxFactory;

        [Inject]
        public void Construct(
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
            _purchaseService.OnCartPurchased += HandleCartPurchased;

        public void Dispose() =>
            _purchaseService.OnCartPurchased -= HandleCartPurchased;

        private void HandleCartPurchased(IReadOnlyList<string> toyIds) =>
            StartCoroutine(SpawnSequentially(toyIds));

        private IEnumerator SpawnSequentially(IReadOnlyList<string> toyIds)
        {
            foreach (string toyId in toyIds)
            {
                SpawnBox(toyId);
                yield return new WaitForSeconds(_spawnInterval);
            }
        }

        private void SpawnBox(string toyId)
        {
            var toy = _catalog.GetToyById(toyId);
            if (toy == null)
            {
                Debug.LogWarning($"[CartDeliveryController] ToyData not found: {toyId}");
                return;
            }

            BoxContainer box = _boxFactory.Create();
            box.transform.position = _deliveryPoint.GetSpawnPosition();
            box.SetupBox(toy);
        }
    }
}