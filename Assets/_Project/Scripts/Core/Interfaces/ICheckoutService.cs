using System;

namespace ToyShop.Core.Interfaces
{
    public interface ICheckoutService
    {
        int QueueLength { get; }

        void EnqueueNpc(INpcController npc);
        void DequeueNpc(INpcController npc);

        bool IsFirstInQueue(INpcController npc);
        void ProcessCheckout(INpcController npc);

        event Action<INpcController> OnCheckoutCompleted;
    }
}