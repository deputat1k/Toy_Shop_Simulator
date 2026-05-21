using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.Items
{
    // Attach to the Box prefab alongside BoxContainer and KinematicGrabPhysics
    public class BoxEmptyFeedbackHandler : MonoBehaviour
    {
        private BoxContainer _boxContainer;
        private IItemGrabbable _grabbable;
        private IHudNotificationService _notification;

        private const string EmptyBoxMessage = "Box is empty!";

        [Inject]
        public void Construct(IHudNotificationService notification)
        {
            _notification = notification;
        }

        private void Awake()
        {
            _boxContainer = GetComponent<BoxContainer>();
            _grabbable = GetComponent<IItemGrabbable>();
        }

        private void Start()
        {
            // Start fires after Inject — _notification is guaranteed to be set
            if (_grabbable != null)
                _grabbable.OnGrabbed += HandleGrabbed;
        }

        private void OnDestroy()
        {
            if (_grabbable != null)
                _grabbable.OnGrabbed -= HandleGrabbed;
        }

        private void HandleGrabbed()
        {
            if (_boxContainer != null && _boxContainer.ItemCount == 0)
                _notification?.ShowMessage(EmptyBoxMessage, Color.red);
        }
    }
}