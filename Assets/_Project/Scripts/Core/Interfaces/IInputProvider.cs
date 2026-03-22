using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IInputProvider
    {
        Vector2 GetMovementDirection();
        bool IsInteractActionTriggered();
        bool IsThrowActionTriggered();
    }
}