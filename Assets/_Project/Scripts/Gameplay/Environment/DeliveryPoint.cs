using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Environment
{
    public class DeliveryPoint : MonoBehaviour, IDeliveryPointProvider
    {
        // Return the position slightly above the ground
        public Vector3 GetSpawnPosition() => transform.position + Vector3.up * 1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(transform.position, new Vector3(1, 0.1f, 1));
        }
    }
}