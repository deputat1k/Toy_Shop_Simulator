using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using ToyShop.Core.SaveSystem;
using ToyShop.Gameplay.Factories;
using UnityEngine;

namespace ToyShop.Gameplay.SaveSystem
{
    public class ShelfSaveHandler : ISaveHandler
    {
        private readonly IPointOfInterestProvider _pointsOfInterest;
        private readonly ICatalogService _catalog;
        private readonly ToyFactory _toyFactory;

        public ShelfSaveHandler(
            IPointOfInterestProvider pointsOfInterest,
            ICatalogService catalog,
            ToyFactory toyFactory)
        {
            _pointsOfInterest = pointsOfInterest;
            _catalog = catalog;
            _toyFactory = toyFactory;
        }

        public void OnSave(GameSaveData saveData)
        {
            saveData.ShelfSlots = new List<ShelfSlotSaveData>();
            IShelfSlot[] slots = _pointsOfInterest.GetAllShelfSlots();

            for (int i = 0; i < slots.Length; i++)
            {
                IShelfSlot slot = slots[i];
                if (!slot.IsOccupied) continue;

                if (slot.CurrentItem is IToyDataHolder holder && holder.ToyData != null)
                {
                    saveData.ShelfSlots.Add(new ShelfSlotSaveData
                    {
                        SlotIndex = i,
                        ToyId = holder.ToyData.Id
                    });
                }
            }
        }

        public void OnLoad(GameSaveData saveData)
        {
            IShelfSlot[] slots = _pointsOfInterest.GetAllShelfSlots();

            ClearAllSlots(slots);

            if (saveData.ShelfSlots == null || saveData.ShelfSlots.Count == 0) return;

            RestoreSlots(slots, saveData.ShelfSlots);
        }

        private void ClearAllSlots(IShelfSlot[] slots)
        {
            foreach (IShelfSlot slot in slots)
            {
                if (!slot.IsOccupied) continue;

                IPlaceable item = slot.CurrentItem;
                slot.Free(); // unsubscribe from events before destroying

                if (item is MonoBehaviour mb)
                    Object.Destroy(mb.gameObject);
            }
        }

        private void RestoreSlots(IShelfSlot[] slots, List<ShelfSlotSaveData> savedSlots)
        {
            foreach (ShelfSlotSaveData slotData in savedSlots)
            {
                if (slotData.SlotIndex < 0 || slotData.SlotIndex >= slots.Length)
                {
                    Debug.LogWarning($"[ShelfSaveHandler] Invalid slot index: {slotData.SlotIndex}");
                    continue;
                }

                var toyData = _catalog.GetToyById(slotData.ToyId);
                if (toyData == null)
                {
                    Debug.LogWarning($"[ShelfSaveHandler] ToyData not found for id: {slotData.ToyId}");
                    continue;
                }

                IShelfSlot slot = slots[slotData.SlotIndex];

                IItemGrabbable item = _toyFactory.Create(
                    toyData,
                    slot.SlotTransform.position,
                    slot.SlotTransform.rotation);

                if (item is IPlaceable placeable)
                    slot.Occupy(placeable);
                else
                    Debug.LogError($"[ShelfSaveHandler] Created item is not IPlaceable. ToyId: {slotData.ToyId}");
            }
        }
    }
}