using System;
using System.Collections.Generic;
using UnityEngine;

namespace ToyShop.Core.SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        // Increment when save format changes — used for future migration
        public int SaveVersion = 1;

        // --- Player ---
        public int PlayerBalance;
        public Vector3 PlayerPosition;

        // --- Environment ---
        public List<ShelfSlotSaveData> ShelfSlots = new List<ShelfSlotSaveData>();
        public List<BoxSaveData> Boxes = new List<BoxSaveData>();

        // --- Future extensions (uncomment when implementing) ---
        // public ShopProgressSaveData ShopProgress;
        // public List<NpcSaveData> ActiveNpcs = new List<NpcSaveData>();
    }
}