using UnityEngine;

namespace ToyShop.Data
{
    [CreateAssetMenu(fileName = "NewToyData", menuName = "ToyShop/Toy Data", order = 1)]
    public class ToyData : ScriptableObject
    {
        [Header("Basic information")]
        public string Id;
        public string DisplayName;
        [TextArea]
        public string Description;

        [Header("Economy")]
        public int PurchasePrice; 
        public int SellPrice;     

        [Header("Visual")]
        public GameObject Prefab;
        public Sprite Icon;
    }
}