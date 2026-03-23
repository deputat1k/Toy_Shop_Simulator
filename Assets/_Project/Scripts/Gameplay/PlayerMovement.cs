using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace ToyShop.Gameplay
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController _controller;
        private Vector3 _velocity;

        private IInputProvider _inputProvider;

        private void Awake()
        { 
            _controller = GetComponent<CharacterController>();
            _inputProvider = GetComponent<IInputProvider>();
        }

        private void Update()
        {
            MovePlayer();
            ApplyGravity();
        }

        private void MovePlayer()
        {

            if (_inputProvider == null) return;

            Vector2 input = _inputProvider.GetMovementDirection();

            Vector3 move = transform.right * input.x + transform.forward * input.y;
            _controller.Move(move * (speed * Time.deltaTime));
        }


        private void ApplyGravity()
        {
            // Проста гравітація, щоб гравець не літав
            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}