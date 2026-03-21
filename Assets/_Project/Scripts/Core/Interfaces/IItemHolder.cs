using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IItemHolder
    {
        Transform GetHoldTransform();

        Vector3 Velocity { get; }

        IItemGrabbable HeldItem { get; set; }
    }
}