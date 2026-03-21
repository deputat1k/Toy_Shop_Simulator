using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Infrastructure
{
    public class PhysicsRaycastScanner : IInteractionScanner
    {
        public IInteractable Scan(Transform origin, float range, LayerMask layerMask)
        {
            // We draw a ray only in the editor
#if UNITY_EDITOR
            Debug.DrawRay(origin.position, origin.forward * range, Color.green, 0.1f);
#endif

            if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, range, layerMask))
            {
                return hit.collider.GetComponentInParent<IInteractable>();
            }

            return null; // Nothing was found
        }
    }
}