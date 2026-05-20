using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay
{
    public class MouseLook : MonoBehaviour
    {
        [Header("Look Settings")]
        [SerializeField] private float _mouseSensitivity = 200f;
        [SerializeField] private Transform _playerBody;

        private float _xRotation;
        private IInputProvider _inputProvider;

        [Inject]
        public void Construct(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        // Start() removed — CursorController handles initial cursor state

        private void Update()
        {
            Vector2 lookDelta = _inputProvider.GetLookDelta();

            float mouseX = lookDelta.x * _mouseSensitivity * Time.deltaTime;
            float mouseY = lookDelta.y * _mouseSensitivity * Time.deltaTime;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            _playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}