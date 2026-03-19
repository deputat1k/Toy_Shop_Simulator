using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Items
{

    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class KinematicGrabPhysics : MonoBehaviour, IItemGrabbable
    {
        private Rigidbody _rigidbody;
        private Collider _collider;

        public bool IsHeld { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public void Grab(Transform holdPointTransform)
        {
            IsHeld = true;

          
            _rigidbody.isKinematic = true;
            _collider.enabled = false;

            // Робимо предмет дочірнім до камери (точки утримання)
            transform.SetParent(holdPointTransform);

            // Центруємо предмет рівно в точці
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public void Drop()
        {
            IsHeld = false;

           
            transform.SetParent(null);

            // Вмикаємо гравітацію та колізію назад
            _rigidbody.isKinematic = false;
            _collider.enabled = true;
        }
    }
}