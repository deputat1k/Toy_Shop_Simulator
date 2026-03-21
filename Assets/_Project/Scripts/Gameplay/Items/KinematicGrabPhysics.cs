using System;
using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class KinematicGrabPhysics : MonoBehaviour, IItemGrabbable
    {
        [Header("Settings")]
        [SerializeField] private float _throwMultiplier = 1f;
        [SerializeField] private string _heldLayerName = "HeldItem";

        private Rigidbody _rigidbody;
        private int _originalLayer;
        private int _heldLayer;

        // Реалізація подій з інтерфейсу
        public event Action OnGrabbed;
        public event Action OnDropped;
        public event Action OnThrown;

        public bool IsHeld { get; private set; }
        public IItemHolder CurrentHolder { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _heldLayer = LayerMask.NameToLayer(_heldLayerName);

            if (_heldLayer == -1)
            {
                Debug.LogError($"Шар '{_heldLayerName}' не знайдено! Створи його в налаштуваннях Unity.");
            }
        }

        public void Grab(IItemHolder holder)
        {
            if (IsHeld) return; // Guard clause

            IsHeld = true;
            CurrentHolder = holder;


            holder.HeldItem = this;

            _originalLayer = gameObject.layer;
            if (_heldLayer != -1) gameObject.layer = _heldLayer;

            _rigidbody.isKinematic = true;

            Transform holdPoint = holder.GetHoldTransform();
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            OnGrabbed?.Invoke(); 
        }

        public void Drop()
        {
            if (!IsHeld) return;

          
            gameObject.layer = _originalLayer;
            transform.SetParent(null);
            _rigidbody.isKinematic = false;

           
            if (CurrentHolder != null && CurrentHolder.HeldItem == (IItemGrabbable)this)
            {
                CurrentHolder.HeldItem = null;
            }

            CurrentHolder = null;
            IsHeld = false;

            OnDropped?.Invoke();
        }

        public void Throw(Vector3 appliedForce)
        {
            if (!IsHeld || CurrentHolder == null) return;

            Vector3 throwVelocity = CurrentHolder.Velocity;

            Drop();

            Vector3 finalForce = (appliedForce * _throwMultiplier) + throwVelocity;

            _rigidbody.AddForce(finalForce, ForceMode.Impulse);

            OnThrown?.Invoke();
        }
    }
}