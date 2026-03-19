using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    // Контракт виключно для предметів, які фізично можна взяти в руки
    public interface IItemGrabbable
    {
        bool IsHeld { get; }
        void Grab(Transform holdPointTransform);
        void Drop();
    }
}