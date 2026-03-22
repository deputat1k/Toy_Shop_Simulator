using UnityEngine;

namespace ToyShop.Data
{
    // creating toys
    [CreateAssetMenu(fileName = "NewToyData", menuName = "Toy Shop/Toy Data", order = 1)]
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
        public GameObject prefab;     // 3D model of a toy that will stand on the shelves³
        public Sprite icon;           // 2D icon for tablet for shopping
    }
}
