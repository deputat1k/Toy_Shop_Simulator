using UnityEngine;
using System;
namespace ToyShop.Core.Interfaces
{
    public interface IPlaceable
    {
        void PlaceAt(Transform targetTransform);
        event Action OnRemovedFromPlacement;
    }
}