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
            if (holder.HeldItem == null) return;

            if (holder.HeldItem is IContainerProvider provider && provider.TryGetContainer(out var container))
            {
                if (!container.CanExtract)
                {
                    OnEmptyContainerProvided?.Invoke();
                    return;
                }

                if (!TryGetEmptySlot(out var slot))
                {
                    OnShelfFull?.Invoke();
                    return;
                }

                if (container.TryExtract(out var item))
                {
                    slot.Occupy(item);
                }
                return;
            }

         
            if (holder.HeldItem is IPlaceable placeableItem)
            {
                if (!TryGetEmptySlot(out var slot))
                {
                    OnShelfFull?.Invoke();
                    return;
                }

                var itemToPlace = holder.HeldItem;

               
                itemToPlace.Drop();

                slot.Occupy(itemToPlace);
            }
        }

        private bool TryGetEmptySlot(out IShelfSlot emptySlot)
        {
            emptySlot = _slots.FirstOrDefault(slot => !slot.IsOccupied);
            return emptySlot != null;
        }
    }
}