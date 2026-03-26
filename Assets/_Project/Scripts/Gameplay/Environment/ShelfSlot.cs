using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Environment
{
    public class ShelfSlot : MonoBehaviour, IShelfSlot
    {
        public bool IsOccupied { get; private set; }
        public Transform SlotTransform => transform;
        public Quaternion PlacementRotation => transform.rotation;

        private IPlaceable _currentItem;

        // Now we only accept what we can put
        public void Occupy(IPlaceable item)
        {
            IsOccupied = true;
            _currentItem = item;

            item.PlaceAt(SlotTransform);
            item.OnRemovedFromPlacement += HandleItemRemoved;
        }

        private void HandleItemRemoved()
        {
            Free();
        }

        public void Free()
        {
            if (_currentItem != null)
            {
                _currentItem.OnRemovedFromPlacement -= HandleItemRemoved;
                _currentItem = null;
            }
            IsOccupied = false;
        }

        private void OnDestroy()
        {
            if (_currentItem != null)
            {
                _currentItem.OnRemovedFromPlacement -= HandleItemRemoved;
            }
        }
    }
}