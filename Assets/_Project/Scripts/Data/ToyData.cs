using UnityEngine;

namespace ToyShop.Data
{
    [CreateAssetMenu(fileName = "NewToyData", menuName = "ToyShop/Toy Data")]
    public class ToyData : ScriptableObject
    {
        [Header("Basic information")]
        public string toyId;
        public string toyName;
        [TextArea]
        public string description;

        [Header("Economy")]
        public float purchasePrice;
        public float sellPrice;

        [Header("Visual")]
        public GameObject prefab;
        public Sprite icon;
    }
}