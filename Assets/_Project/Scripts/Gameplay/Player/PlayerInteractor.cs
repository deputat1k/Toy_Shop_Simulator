using UnityEngine;
using Zenject;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Player
{
    // Клас сам виступає провайдером точки утримання
    public class PlayerInteractor : MonoBehaviour, IHoldPointProvider, IInteractor
    {
        [Header("Settings")]
        [SerializeField] private float _interactRange = 3f;
        [SerializeField] private LayerMask _interactLayer;
        [SerializeField] private Transform _holdPosition;
        [SerializeField] private Camera _camera;

        private IInputProvider _inputProvider;
        private IInteractionScanner _scanner;
        private IInteractable _currentInteractable;
        public IHoldPointProvider HoldPoint => this;

        [Inject]
        public void Construct(IInputProvider inputProvider, IInteractionScanner scanner, Camera mainCamera)
        {
            _inputProvider = inputProvider;
            _scanner = scanner;
           
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
            if (_currentInteractable == null)
            {
                _currentInteractable = _scanner.Scan(_camera.transform, _interactRange, _interactLayer);

                if (_currentInteractable != null)
                {
                   
                    _currentInteractable.Interact(this);
                }
            }
            else
            {
            
                _currentInteractable.Interact(this);
                _currentInteractable = null;
            }
        }
    }
}