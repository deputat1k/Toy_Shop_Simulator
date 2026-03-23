using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Environment
{
    public class ShelfSlot : MonoBehaviour, IShelfSlot
    {
        public bool IsOccupied { get; private set; }
        public Transform SlotTransform => transform;
        public Quaternion PlacementRotation => transform.rotation;

        private IItemGrabbable _currentItem;

        public void Occupy(IItemGrabbable item)
        {
            IsOccupied = true;
            _currentItem = item;

            if (item is IPlaceable placeable)
            {
                placeable.PlaceAt(SlotTransform);
            }
        }

        public void Free()
        {
            IsOccupied = false;
            _currentItem = null;
        }
    }
}