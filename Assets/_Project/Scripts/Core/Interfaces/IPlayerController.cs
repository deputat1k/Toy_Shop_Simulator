using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IPlayerController
    {
        void EnableInput();
        void DisableInput();

        Transform Transform { get; }

        // Safely moves the player — handles CharacterController internally
        void SetPosition(Vector3 position);
    }
}