using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Items
{
    public class ItemInteractionTrigger : MonoBehaviour, IInteractable
    {
        private IItemGrabbable _grabbable;

        private void Awake()
        {
            _grabbable = GetComponent<IItemGrabbable>();
        }

        public void Interact(IInteractor interactor)
        {
            if (_grabbable == null) return;

            if (interactor is IItemHolder holder)
            {
                // Guard Clause: If the hands are busy, the player cannot take a new item.
                if (holder.HeldItem != null && !_grabbable.IsHeld) return;

                if (!_grabbable.IsHeld)
                {
                    _grabbable.Grab(holder);
                }
                else
                {
                    _grabbable.Drop();
                }
            }
        }

        }
}