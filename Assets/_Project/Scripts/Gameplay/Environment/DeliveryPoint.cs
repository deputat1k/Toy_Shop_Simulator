using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Environment
{
    public class DeliveryPoint : MonoBehaviour, IDeliveryPointProvider
    {
        [SerializeField] private Transform[] _deliveryZones;
        [SerializeField] private float _spawnHeight = 1.8f;

        private int _nextZoneIndex = 0;

        public Vector3 GetSpawnPosition()
        {
            if (_deliveryZones == null || _deliveryZones.Length == 0)
            {
                Debug.LogWarning("[DeliveryPoint] No delivery zones assigned — falling back to own position.");
                return transform.position + Vector3.up * _spawnHeight;
            }

            Transform zone = _deliveryZones[_nextZoneIndex];
            _nextZoneIndex = (_nextZoneIndex + 1) % _deliveryZones.Length;

            return zone.position + Vector3.up * _spawnHeight;
        }

        private void OnDrawGizmos()
        {
            if (_deliveryZones == null) return;

            Gizmos.color = new Color(0f, 1f, 0f, 0.6f);

            foreach (Transform zone in _deliveryZones)
            {
                if (zone == null) continue;

                Gizmos.DrawCube(zone.position, new Vector3(0.8f, 0.05f, 0.8f));

                Gizmos.DrawLine(zone.position, zone.position + Vector3.up * _spawnHeight);

                Gizmos.DrawWireSphere(zone.position + Vector3.up * _spawnHeight, 0.15f);
            }
        }
    }
}