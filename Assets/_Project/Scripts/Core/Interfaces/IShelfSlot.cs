using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IShelfSlot
    {
        bool IsOccupied { get; }
        Transform SlotTransform { get; }
        Quaternion PlacementRotation { get; }
        IPlaceable CurrentItem { get; }

        void Occupy(IPlaceable item);
        void Free();
    }
}