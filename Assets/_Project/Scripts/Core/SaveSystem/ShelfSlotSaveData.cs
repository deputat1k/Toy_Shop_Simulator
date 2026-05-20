using System;

namespace ToyShop.Core.SaveSystem
{
    [Serializable]
    public class ShelfSlotSaveData
    {
        // Index in array returned by IPointOfInterestProvider.GetAllShelfSlots()
        public int SlotIndex;

        // Matches ToyData.Id from ToyDatabase
        public string ToyId;
    }
}