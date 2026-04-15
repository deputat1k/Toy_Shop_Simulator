using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Player
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private PlayerMovement _movement;
        private PlayerInteractor _interactor;
        private MouseLook _mouseLook; // 1. Додали змінну для камери

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _interactor = GetComponent<PlayerInteractor>();

            // 2. Шукаємо MouseLook (навіть якщо він висить глибше, на камері)
            _mouseLook = GetComponentInChildren<MouseLook>();
        }

        public void DisableInput()
        {
            if (_movement != null) _movement.enabled = false;
            if (_interactor != null) _interactor.enabled = false;

            // 3. Вимикаємо обертання камери
            if (_mouseLook != null) _mouseLook.enabled = false;
        }

        public void EnableInput()
        {
            if (_movement != null) _movement.enabled = true;
            if (_interactor != null) _interactor.enabled = true;

            // 4. Вмикаємо обертання камери
            if (_mouseLook != null) _mouseLook.enabled = true;
        }
    }
}