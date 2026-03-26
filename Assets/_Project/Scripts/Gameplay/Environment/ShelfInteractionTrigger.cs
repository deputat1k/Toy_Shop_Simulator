using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Environment
{
    public class ShelfInteractionTrigger : MonoBehaviour, IInteractable
    {
    
        [SerializeField] private ShelfManager _shelfManager;

        public void Interact(IInteractor interactor)
        {
            if (interactor is IItemHolder holder && _shelfManager != null)
            {
               
                _shelfManager.ProcessInteraction(holder);
            }
        }
    }
}