using UnityEngine;
using Zenject;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Player
{
    // Клас сам виступає провайдером точки утримання
    public class PlayerInteractor : MonoBehaviour, IHoldPointProvider
    {
        [Header("Settings")]
        [SerializeField] private float _interactRange = 3f;
        [SerializeField] private LayerMask _interactLayer;
        [SerializeField] private Transform _holdPosition;

        private IInputProvider _inputProvider;
        private IInteractionScanner _scanner;
        private Camera _camera;

        private IInteractable _currentInteractable;

        [Inject]
        public void Construct(IInputProvider inputProvider, IInteractionScanner scanner, Camera mainCamera)
        {
            _inputProvider = inputProvider;
            _scanner = scanner;
            _camera = mainCamera;
        }

        
        public Transform GetHoldTransform() => _holdPosition;

        private void Update()
        {
            if (_inputProvider.IsInteractActionTriggered())
            {
                HandleInteraction();
            }
        }

        private void HandleInteraction()
        {
            // Відпускаємо предмет
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(null);
                _currentInteractable = null;
                return;
            }

            // Беремо предмет
            _currentInteractable = _scanner.Scan(_camera.transform, _interactRange, _interactLayer);
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(this);
            }
        }
    }
}