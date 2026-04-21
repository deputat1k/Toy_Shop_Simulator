using System;
using ToyShop.Core.Interfaces;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace ToyShop.Gameplay.Items
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class KinematicGrabPhysics : MonoBehaviour, IItemGrabbable, IPlaceable, IContainerProvider
    {
        [Header("Settings")]
        [SerializeField] private float _throwMultiplier = 1f;
        [SerializeField] private string _heldLayerName = "HeldItem";

        [Header("Placement Fixes")]
        [SerializeField] private float _placementHeightOffset = 0f;

        private Rigidbody _rigidbody;
        private int _originalLayer;
        private int _heldLayer;

        // Implementing events from the interface
        public event Action OnGrabbed;
        public event Action OnDropped;
        public event Action OnThrown;
        public event Action OnRemovedFromPlacement;

        public bool IsHeld { get; private set; }
        public IItemHolder CurrentHolder { get; private set; }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _heldLayer = LayerMask.NameToLayer(_heldLayerName);

            // Keep the original layer
            _originalLayer = gameObject.layer;

            if (_heldLayer == -1)
            {
                Debug.LogError($"Layer '{_heldLayerName}' not found! Create it in Unity settings.");
            }
        }

        public void PlaceAt(Transform targetTransform)
        {
            _rigidbody.isKinematic = true;
            transform.position = targetTransform.position + Vector3.up * _placementHeightOffset;
            transform.rotation = targetTransform.rotation;
        }

        public void Grab(IItemHolder holder)
        {
            if (IsHeld) return;

            IsHeld = true;
            CurrentHolder = holder;


            holder.HeldItem = this;


            if (_heldLayer != -1) gameObject.layer = _heldLayer;

            _rigidbody.isKinematic = true;
            Transform holdPoint = holder.GetHoldTransform();
            transform.SetParent(holdPoint);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            OnGrabbed?.Invoke();
            OnRemovedFromPlacement?.Invoke();
        }

        public void Drop()
        {
            if (!IsHeld) return;


            gameObject.layer = _originalLayer;


            transform.SetParent(null);
            _rigidbody.isKinematic = false;


            if (CurrentHolder != null)
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

       

        public bool TryGetContainer(out IItemContainer container)
        {
            container = GetComponent<IItemContainer>();
            return container != null;
        }
    }
}