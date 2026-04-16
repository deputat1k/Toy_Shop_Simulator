using UnityEngine;
using Zenject;
using ToyShop.Core.Interfaces;
using ToyShop.Data;

namespace ToyShop.Gameplay.Factories
{
    
    public class ToyFactory
    {
        private readonly IInstantiator _instantiator;

        public ToyFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public IItemGrabbable Create(ToyData toyData, Vector3 position, Quaternion rotation)
        {
            if (toyData == null || toyData.Prefab == null)
            {
                Debug.LogError("ToyFactory: ToyData or prefab missing!");
                return null;
            }

            
            GameObject instance = _instantiator.InstantiatePrefab(toyData.Prefab, position, rotation, null);

           

            return instance.GetComponent<IItemGrabbable>();
        }
    }
}