using System;
using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface ICheckoutService
    {
        int QueueLength { get; }

        void EnqueueNpc(INpcController npc);
        void DequeueNpc(INpcController npc);

        bool IsFirstInQueue(INpcController npc);
        INpcController GetFirstInQueue();
        void ProcessCheckout(INpcController npc);

        Vector3 GetNpcQueuePosition(INpcController npc);

        // Position NPCs should face while waiting in queue
        Vector3 GetCounterFacingPosition();

        event Action<INpcController> OnCheckoutCompleted;
        event Action OnQueueChanged;
    }
}