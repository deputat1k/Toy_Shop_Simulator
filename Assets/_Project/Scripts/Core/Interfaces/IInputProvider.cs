using System;
using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IInputProvider
    {
        Vector2 GetMovementDirection();

        // Raw mouse delta — sensitivity and time scaling applied by consumer
        Vector2 GetLookDelta();

        bool IsInteractActionTriggered();
        bool IsThrowActionTriggered();

        event Action OnTabletTogglePressed;
        event Action OnPausePressed;
    }
}