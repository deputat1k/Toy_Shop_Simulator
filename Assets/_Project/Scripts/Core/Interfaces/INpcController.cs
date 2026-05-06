using System;
using ToyShop.Data;
using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface INpcController
    {
        // Navigation
        void MoveTo(Vector3 destination);
        bool HasReachedDestination();

        // State
        void ChangeState(INpcState newState);

        // Item interaction
        ToyData SelectedToy { get; set; }
        bool HasItem { get; set; }

        // Shelf interaction
        IShelfSlot TargetSlot { get; set; }

        // World references
        Transform Transform { get; }

        // Pool lifecycle
        event Action OnReadyToReturn;
        void NotifyReadyToReturn();
    }
}