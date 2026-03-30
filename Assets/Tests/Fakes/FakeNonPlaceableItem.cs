using System;
using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Tests.Fakes
{
    public class FakeNonPlaceableItem : IItemGrabbable
    {
        public event Action OnGrabbed;
        public event Action OnDropped;
        public event Action OnThrown;

        public bool IsHeld { get; set; }
        public IItemHolder CurrentHolder { get; set; }

        public void Grab(IItemHolder holder) { }
        public void Drop() { }
        public void Throw(Vector3 appliedForce) { }
    }
}