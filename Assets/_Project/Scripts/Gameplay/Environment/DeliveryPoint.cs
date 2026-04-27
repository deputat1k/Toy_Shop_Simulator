using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Environment
{
    public class DeliveryPoint : MonoBehaviour, IDeliveryPointProvider
    {
        [SerializeField] private float _spawnHeightOffset = 1f;

        public Vector3 GetSpawnPosition() =>
            transform.position + Vector3.up * _spawnHeightOffset;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawCube(transform.position, new Vector3(1f, 0.1f, 1f));
        }
    }
}