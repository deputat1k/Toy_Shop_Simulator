using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IShelfSlot
    {
        bool IsOccupied { get; }
        Transform SlotTransform { get; }
        Quaternion PlacementRotation { get; }

        void Occupy(IPlaceable item);
        void Free();
    }
}
