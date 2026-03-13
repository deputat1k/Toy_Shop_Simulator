using UnityEngine;

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

        private void Awake()
        { 
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            MovePlayer();
            ApplyGravity();
        }

        private void MovePlayer()
        {
           
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

          
            Vector3 move = transform.right * x + transform.forward * z;

            // Нормалізуємо вектор, щоб гравець не бігав по діагоналі швидше
            if (move.magnitude > 1f) move.Normalize();

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