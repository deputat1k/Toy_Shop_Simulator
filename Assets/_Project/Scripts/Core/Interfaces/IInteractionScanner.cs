using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IInteractionScanner
    {
        IInteractable Scan(Transform origin, float range, LayerMask layerMask);
    }
}