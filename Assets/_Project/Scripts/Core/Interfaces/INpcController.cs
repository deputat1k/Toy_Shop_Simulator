using System;
using ToyShop.Data;
using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface INpcController
    {
        void MoveTo(Vector3 destination);
        bool HasReachedDestination();
        void ChangeState(INpcState newState);

        ToyData SelectedToy { get; set; }
        bool HasItem { get; set; }
        IShelfSlot TargetSlot { get; set; }
        Transform Transform { get; }

        void FaceDirection(Vector3 targetPosition);
        bool IsFacingTarget(Vector3 targetPosition, float angleThreshold = 10f);
        void SetAgentRotationEnabled(bool enabled);

        // Animation control — implemented by NpcController, called by states
        void PlayInteractAnimation();
        void StopInteractAnimation();

        void ShowItemVisual();
        void HideItemVisual();

        event Action OnReadyToReturn;
        void NotifyReadyToReturn();
    }
}