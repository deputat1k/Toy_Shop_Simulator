using UnityEngine;
using Zenject;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float gravity = -9.81f;

        private CharacterController _controller;
        private Vector3 _velocity;

        private IInputProvider _inputProvider;

       
        [Inject]
        public void Construct(IInputProvider inputProvider)
        {
            _inputProvider = inputProvider;
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Vector2 input = _inputProvider.GetMovementDirection();
            Vector3 move = transform.right * input.x + transform.forward * input.y;
            _controller.Move(move * (speed * Time.deltaTime));

            if (_controller.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}