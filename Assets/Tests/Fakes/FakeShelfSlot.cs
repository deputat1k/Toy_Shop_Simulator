using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Tests.Fakes
{
    public class FakeShelfSlot : IShelfSlot
    {
        public bool IsOccupied { get; private set; }
        public IPlaceable OccupiedBy { get; private set; }
        public Transform SlotTransform => null;
        public Quaternion PlacementRotation => Quaternion.identity;

        public void Occupy(IPlaceable item)
        {
            IsOccupied = true;
            OccupiedBy = item;
        }

        public void Free()
        {
            IsOccupied = false;
            OccupiedBy = null;
        }

        public void ForceOccupy()
        {
            IsOccupied = true;
        }
    }
}