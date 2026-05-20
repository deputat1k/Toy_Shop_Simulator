using System;
using System.Linq;
using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay.Environment
{
    public class ShelfManager : MonoBehaviour, IShelfManager
    {
        private IShelfSlot[] _slots;

        // Field injection with Optional — valid Zenject syntax unlike [Inject(Optional=true)] on methods
        [Inject(Optional = true)]
        private IHudNotificationService _notification;

        public event Action OnShelfFull;
        public event Action OnEmptyContainerProvided;

        private void Awake()
        {
            _slots = GetComponentsInChildren<IShelfSlot>();
        }

        public bool HasEmptySlot => _slots.Any(slot => !slot.IsOccupied);

        public void ProcessInteraction(IItemHolder holder)
        {
            var heldItem = holder.HeldItem;
            if (heldItem == null) return;

            if (TryHandleContainer(heldItem)) return;

            TryHandleSingleItem(holder);
        }

        private bool TryGetEmptySlot(out IShelfSlot emptySlot)
        {
            emptySlot = _slots.FirstOrDefault(slot => !slot.IsOccupied);
            return emptySlot != null;
        }

        private bool TryHandleContainer(IItemGrabbable heldItem)
        {
            if (!(heldItem is IContainerProvider provider &&
                  provider.TryGetContainer(out var container)))
                return false;

            if (!container.CanExtract)
            {
                _notification?.ShowMessage("Box is empty!", Color.red);
                OnEmptyContainerProvided?.Invoke();
                return true;
            }

            if (!TryGetEmptySlot(out var slot))
            {
                OnShelfFull?.Invoke();
                return true;
            }

            if (container.TryExtract(out var item) && item is IPlaceable placeable)
                slot.Occupy(placeable);

            return true;
        }

        private void TryHandleSingleItem(IItemHolder holder)
        {
            if (!TryGetEmptySlot(out var slot))
            {
                OnShelfFull?.Invoke();
                return;
            }

            if (holder.HeldItem is IPlaceable placeable)
            {
                holder.HeldItem.Drop();
                slot.Occupy(placeable);
            }
        }
    }
}