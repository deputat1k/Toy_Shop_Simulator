using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.Environment
{
    public class CheckoutCounter : MonoBehaviour, ICheckoutQueue, IInteractable
    {
        [Header("Queue Positions")]
        [Tooltip("Index 0 = counter, 1 = first in line, 2 = second, etc.")]
        [SerializeField] private Transform[] _queuePositions;

      

        private ICheckoutService _checkoutService;

        public int Capacity => _queuePositions?.Length ?? 0;

        [Inject]
        public void Construct(ICheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public Vector3 GetPositionAt(int index)
        {
            if (_queuePositions == null || _queuePositions.Length == 0)
            {
                Debug.LogWarning("CheckoutCounter: No queue positions assigned.");
                return transform.position;
            }

            int safeIndex = Mathf.Clamp(index, 0, _queuePositions.Length - 1);
            return _queuePositions[safeIndex].position;
        }

      

        public void Interact(IInteractor interactor)
        {
            if (_checkoutService.QueueLength == 0) return;

            INpcController firstNpc = _checkoutService.GetFirstInQueue();
            if (firstNpc == null) return;

            _checkoutService.ProcessCheckout(firstNpc);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_queuePositions == null || _queuePositions.Length == 0)
                Debug.LogWarning("CheckoutCounter: Queue positions are not assigned.");

        }

        private void OnDrawGizmos()
        {
            if (_queuePositions != null)
            {
                for (int i = 0; i < _queuePositions.Length; i++)
                {
                    if (_queuePositions[i] == null) continue;
                    Gizmos.color = i == 0 ? Color.yellow : Color.cyan;
                    Gizmos.DrawSphere(_queuePositions[i].position, 0.25f);
                    UnityEditor.Handles.Label(
                        _queuePositions[i].position + Vector3.up * 0.5f,
                        i == 0 ? "Counter" : $"Queue {i}");
                }
            }

            
        }
#endif
    }
}