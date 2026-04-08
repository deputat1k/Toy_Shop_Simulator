using System;
using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Tests.Fakes
{
    public class FakePlaceableItem : IItemGrabbable, IPlaceable
    {
        public event Action OnGrabbed;
        public event Action OnDropped;
        public event Action OnThrown;
        public event Action OnRemovedFromPlacement;

        public bool IsHeld { get; set; }
        public IItemHolder CurrentHolder { get; set; }
        public bool WasDropped { get; private set; }

        public void Drop()
        {
            WasDropped = true;
            IsHeld = false;
        }

        public void PlaceAt(Transform targetTransform) { }
        public void Grab(IItemHolder holder) { }
        public void Throw(Vector3 appliedForce) { }
    }
}