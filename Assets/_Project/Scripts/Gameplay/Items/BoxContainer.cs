using System;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using ToyShop.Gameplay.Factories;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.Items
{
    public class BoxContainer : MonoBehaviour, IItemContainer
    {
        [Header("Container Settings")]
        [SerializeField] private ToyData _toyData;
        [SerializeField] private int _itemCount = 4;

        private ToyFactory _toyFactory;

        public event Action OnItemExtracted;
        public event Action OnContainerEmpty;

        public bool CanExtract => _itemCount > 0 && _toyData != null && _toyData.prefab != null;

        [Inject]
        public void Construct(ToyFactory toyFactory)
        {
            _toyFactory = toyFactory;
        }

        public bool TryExtract(out IItemGrabbable extractedItem)
        {
            if (!CanExtract)
            {
                extractedItem = null;
                return false;
            }

            _itemCount--;

         
            extractedItem = _toyFactory.Create(_toyData, transform.position, transform.rotation);

            OnItemExtracted?.Invoke();

            if (_itemCount <= 0)
            {
                OnContainerEmpty?.Invoke();
            }

            return true;
        }
    }
}