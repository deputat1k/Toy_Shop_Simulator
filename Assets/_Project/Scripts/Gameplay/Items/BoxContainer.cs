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

       
        public class Factory : PlaceholderFactory<BoxContainer> { }

        public bool CanExtract => _itemCount > 0 && _toyData != null && _toyData.Prefab != null;

        [Inject]
        public void Construct(ToyFactory toyFactory)
        {
            _toyFactory = toyFactory;
        }


        public void SetupBox(ToyData toyData)
        {
            _toyData = toyData;
            _itemCount = 4;
        }

      
        public bool TryExtract(out IItemGrabbable extractedItem)
        {
            if (!CanExtract)
            {
                extractedItem = null;
                return false;
            }

            _itemCount--;

         
            extractedItem = _toyFactory.Create(_toyData, transform.position + Vector3.up * 0.5f, transform.rotation);

            OnItemExtracted?.Invoke();

            if (_itemCount <= 0)
            {
                OnContainerEmpty?.Invoke();
            }

            return true;
        }
    }
}