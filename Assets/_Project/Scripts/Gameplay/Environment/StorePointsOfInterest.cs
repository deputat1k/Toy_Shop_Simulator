using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Environment
{
    public class StorePointsOfInterest : MonoBehaviour, IPointOfInterestProvider
    {
        [Header("Navigation Points")]
        [SerializeField] private Transform _entryPoint;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private Transform _checkoutPoint;

        [Header("Shelves")]
        [SerializeField] private ShelfManager[] _shelfManagers;

        // Cache shelf slot references (not occupancy — that's dynamic)
        private IShelfSlot[] _cachedSlots;

        public Vector3 GetEntryPoint() => _entryPoint.position;
        public Vector3 GetExitPoint() => _exitPoint.position;
        public Vector3 GetCheckoutPoint() => _checkoutPoint.position;

        public IShelfSlot[] GetAllShelfSlots()
        {
            // Cache slot references once — occupancy is checked dynamically by Brain
            if (_cachedSlots != null) return _cachedSlots;

            var allSlots = new System.Collections.Generic.List<IShelfSlot>();
            foreach (ShelfManager shelf in _shelfManagers)
            {
                if (shelf == null) continue;
                allSlots.AddRange(shelf.GetComponentsInChildren<IShelfSlot>());
            }

            _cachedSlots = allSlots.ToArray();

            // Warn if no slots found — likely Inspector not configured
            if (_cachedSlots.Length == 0)
                Debug.LogWarning("StorePointsOfInterest: No ShelfSlots found. Check _shelfManagers in Inspector.");

            return _cachedSlots;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_entryPoint == null)
                Debug.LogWarning("StorePointsOfInterest: Entry Point is not assigned.");
            if (_exitPoint == null)
                Debug.LogWarning("StorePointsOfInterest: Exit Point is not assigned.");
            if (_checkoutPoint == null)
                Debug.LogWarning("StorePointsOfInterest: Checkout Point is not assigned.");
            if (_shelfManagers == null || _shelfManagers.Length == 0)
                Debug.LogWarning("StorePointsOfInterest: No ShelfManagers assigned.");
        }

        private void OnDrawGizmos()
        {
            DrawPoint(_entryPoint, Color.green, "Entry");
            DrawPoint(_exitPoint, Color.red, "Exit");
            DrawPoint(_checkoutPoint, Color.yellow, "Checkout");
        }

        private void DrawPoint(Transform point, Color color, string label)
        {
            if (point == null) return;
            Gizmos.color = color;
            Gizmos.DrawSphere(point.position, 0.3f);
            UnityEditor.Handles.Label(point.position + Vector3.up * 0.5f, label);
        }
#endif
    }
}