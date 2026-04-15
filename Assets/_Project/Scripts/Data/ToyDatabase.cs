using System.Collections.Generic;
using UnityEngine;

namespace ToyShop.Data
{
    [CreateAssetMenu(fileName = "MainToyDatabase", menuName = "ToyShop/Toy Database")]
    public class ToyDatabase : ScriptableObject
    {
        public List<ToyData> Toys = new List<ToyData>();
    }
}