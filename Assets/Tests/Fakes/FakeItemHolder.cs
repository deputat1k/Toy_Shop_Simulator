using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Tests.Fakes
{
    public class FakeItemHolder : IItemHolder
    {
        public IItemGrabbable HeldItem { get; set; }

        public Vector3 Velocity { get; set; } = Vector3.zero;

        public Transform GetHoldTransform() => null;
    }
}