using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.Environment
{
    public class TrashBin : MonoBehaviour, IInteractable
    {
        private IHudNotificationService _notification;

        [Inject]
        public void Construct(IHudNotificationService notification)
        {
            _notification = notification;
        }

        public void Interact(IInteractor interactor)
        {
            if (!(interactor is IItemHolder holder)) return;
            if (holder.HeldItem == null) return;

            IItemGrabbable heldItem = holder.HeldItem;

            // Only boxes (IContainerProvider) can be trashed
            if (!(heldItem is IContainerProvider provider &&
                  provider.TryGetContainer(out IItemContainer container)))
            {
                _notification.ShowMessage("Only boxes can be trashed!", Color.yellow);
                return;
            }

            // Reject non-empty boxes
            if (container.CanExtract)
            {
                _notification.ShowMessage("Box is not empty!", Color.yellow);
                return;
            }

            // Save reference before Drop() clears HeldItem
            MonoBehaviour boxMb = heldItem as MonoBehaviour;

            heldItem.Drop();

            if (boxMb != null)
                Destroy(boxMb.gameObject);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.6f, 0.3f, 0f, 0.5f);
            Gizmos.DrawCube(transform.position + Vector3.up * 0.5f, new Vector3(0.8f, 1f, 0.8f));
        }
#endif
    }
}