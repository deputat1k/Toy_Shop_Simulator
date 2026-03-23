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
            if (toyData == null || toyData.prefab == null)
            {
                Debug.LogError("ToyFactory: ToyData або prefab відсутні!");
                return null;
            }

            
            GameObject instance = _instantiator.InstantiatePrefab(toyData.prefab, position, rotation, null);

           

            return instance.GetComponent<IItemGrabbable>();
        }
    }
}