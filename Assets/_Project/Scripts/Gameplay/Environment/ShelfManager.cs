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


        public ShelfManager(IShelfSlot[] slots = null)
        {
            _slots = slots ?? System.Array.Empty<IShelfSlot>();
        }
        private void Awake()
        {
            if (_slots == null || _slots.Length == 0)
            {
                _slots = GetComponentsInChildren<IShelfSlot>();
            }
        }

        public void Initialize(IShelfSlot[] slots)
        {
            _slots = slots;
        }

       
        public bool HasEmptySlot
        {
            get { return _slots.Any(slot => !slot.IsOccupied); }
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