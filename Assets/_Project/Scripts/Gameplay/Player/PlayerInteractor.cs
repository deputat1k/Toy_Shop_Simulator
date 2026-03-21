using UnityEngine;
using Zenject;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay
{
    // RequireComponent гарантуЇ, що на гравц≥ точно Ї CharacterController
    [RequireComponent(typeof(CharacterController))]
    public class PlayerInteractor : MonoBehaviour, IInteractor, IItemHolder
    {
        [Header("Settings")]
        [SerializeField] private float _interactRange = 3f;
        [SerializeField] private LayerMask _interactLayer;
        [SerializeField] private Transform _holdPosition;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _baseThrowForce = 10f;

        private IInputProvider _inputProvider;
        private IInteractionScanner _scanner;
        private CharacterController _characterController;

     
        public IItemGrabbable HeldItem { get; set; }
        public Vector3 Velocity => _characterController.velocity; 

        [Inject]
        public void Construct(IInputProvider inputProvider, IInteractionScanner scanner)
        {
            _inputProvider = inputProvider;
            _scanner = scanner;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
           
            if (_inputProvider.IsThrowActionTriggered() && HeldItem != null)
            {
                HandleThrow();
                return; 
            }

            if (_inputProvider.IsInteractActionTriggered())
            {
                HandleInteraction();
            }
        }

        private void HandleInteraction()
        {
            // якщо вже щось тримаЇмо Ч в≥дпускаЇмо п≥д ноги
            if (HeldItem != null)
            {
                HeldItem.Drop();
                return;
            }

            
            var interactable = _scanner.Scan(_camera.transform, _interactRange, _interactLayer);
            interactable?.Interact(this);
        }

        private void HandleThrow()
        {
            // –ахуЇмо т≥льки вектор напр€мку в≥д камери * базову силу
            Vector3 throwDirection = _camera.transform.forward * _baseThrowForce;

          
            HeldItem.Throw(throwDirection);
        }

        public Transform GetHoldTransform() => _holdPosition;
    }
}