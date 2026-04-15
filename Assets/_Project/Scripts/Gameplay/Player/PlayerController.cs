using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private PlayerMovement _movement;
        private PlayerInteractor _interactor;
        private MouseLook _mouseLook;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _interactor = GetComponent<PlayerInteractor>();

            // Looking for MouseLook
            _mouseLook = GetComponentInChildren<MouseLook>();
        }

        public void DisableInput()
        {
            if (_movement != null) _movement.enabled = false;
            if (_interactor != null) _interactor.enabled = false;

            // Turn off camera rotation
            if (_mouseLook != null) _mouseLook.enabled = false;
        }

        public void EnableInput()
        {
            if (_movement != null) _movement.enabled = true;
            if (_interactor != null) _interactor.enabled = true;

            // Turn on camera rotation
            if (_mouseLook != null) _mouseLook.enabled = true;
        }
    }
}