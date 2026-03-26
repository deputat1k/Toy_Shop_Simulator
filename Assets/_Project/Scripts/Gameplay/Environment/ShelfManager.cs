using System;
using UnityEngine;
using System.Linq;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Environment
{
    public class ShelfManager : MonoBehaviour, IShelfManager
    {
        private IShelfSlot[] _slots;

        public event Action OnShelfFull;
        public event Action OnEmptyContainerProvided;

        private void Awake()
        {
            _slots = GetComponentsInChildren<IShelfSlot>();
        }

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
            if (heldItem is IContainerProvider provider && provider.TryGetContainer(out var container))
            {
                if (!container.CanExtract)
                {
                    OnEmptyContainerProvided?.Invoke();
                    return true; 
                }

                if (!TryGetEmptySlot(out var slot))
                {
                    OnShelfFull?.Invoke();
                    return true;
                }

                if (container.TryExtract(out var item) && item is IPlaceable placeable)
                {
                    slot.Occupy(placeable);
                }
                return true; 
            }
            return false; 
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
                holder.HeldItem.Drop(); // Freeing our hands
                slot.Occupy(placeable);
            }
        }
    }
}