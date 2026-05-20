using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private PlayerMovement _movement;
        private PlayerInteractor _interactor;
        private MouseLook _mouseLook;
        private CharacterController _characterController;

        public Transform Transform => transform;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _interactor = GetComponent<PlayerInteractor>();
            _mouseLook = GetComponentInChildren<MouseLook>();
            _characterController = GetComponent<CharacterController>();
        }

        public void DisableInput()
        {
            if (_movement != null) _movement.enabled = false;
            if (_interactor != null) _interactor.enabled = false;
            if (_mouseLook != null) _mouseLook.enabled = false;
        }

        public void EnableInput()
        {
            if (_movement != null) _movement.enabled = true;
            if (_interactor != null) _interactor.enabled = true;
            if (_mouseLook != null) _mouseLook.enabled = true;
        }

        public void SetPosition(Vector3 position)
        {
            // CharacterController intercepts direct transform.position changes
            // Disable temporarily to allow clean teleportation
            if (_characterController != null) _characterController.enabled = false;
            transform.position = position;
            if (_characterController != null) _characterController.enabled = true;
        }
    }
}