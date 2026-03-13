using UnityEngine;

namespace ToyShop.Data
{
    // створення іграшок
    [CreateAssetMenu(fileName = "NewToyData", menuName = "Toy Shop/Toy Data", order = 1)]
    public class ToyData : ScriptableObject
    {
        [Header("Basic information")]
        public string toyId;          // ID (наприклад "car_red_01")
        public string toyName;        // Назва
        [TextArea]
        public string description;    // Опис іграшки

        [Header("Economy")]
        public float purchasePrice;   // За скільки ми купуємо товар
        public float sellPrice;       // За скільки ми продаємо товар

        [Header("Visual")]
        public GameObject prefab;     // 3D модель іграшки яка буде стояти на полиці
        public Sprite icon;           // Іконка 2D для планшета для покупки 
    }
}
