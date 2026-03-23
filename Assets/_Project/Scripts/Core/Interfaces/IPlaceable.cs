using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IPlaceable
    {
        void PlaceAt(Transform targetTransform);
    }
}