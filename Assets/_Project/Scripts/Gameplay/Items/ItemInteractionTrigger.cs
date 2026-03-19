using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.Items
{

    public class ItemInteractionTrigger : MonoBehaviour, IInteractable
    {
        private IItemGrabbable _grabbable;

        private void Awake()
        {
            // Шукаємо компонент фізики на цьому ж об'єкті
            _grabbable = GetComponent<IItemGrabbable>();

            if (_grabbable == null)
            {
                Debug.LogWarning($"На об'єкті {gameObject.name} немає компонента IItemGrabbable!");
            }
        }

        public void Interact(IInteractor interactor)
        {
            if (_grabbable == null) return;

            // Якщо ми ще не тримаємо предмет
            if (!_grabbable.IsHeld)
            {
                // Перевіряємо, чи має гравець точку для утримання
                if (interactor.HoldPoint != null)
                {
                    _grabbable.Grab(interactor.HoldPoint.GetHoldTransform());
                }
            }
            
            else
            {
                _grabbable.Drop();
            }
        }
    }
}