using System;
using UnityEngine;

namespace ToyShop.Core.SaveSystem
{
    [Serializable]
    public class BoxSaveData
    {
        // Empty string if box has no ToyData assigned
        public string ToyId;
        public int ItemCount;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}