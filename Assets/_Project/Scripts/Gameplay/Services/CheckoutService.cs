using System;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IEconomyService _economy;
        private readonly Queue<INpcController> _queue = new Queue<INpcController>();

        public int QueueLength => _queue.Count;

        public event Action<INpcController> OnCheckoutCompleted;

        public CheckoutService(IEconomyService economy)
        {
            _economy = economy;
        }

        public void EnqueueNpc(INpcController npc)
        {
            if (npc == null)
            {
                Debug.LogError("CheckoutService: attempted to enqueue null NPC.");
                return;
            }

            if (_queue.Contains(npc))
            {
                Debug.LogWarning("CheckoutService: NPC is already in queue.");
                return;
            }

            _queue.Enqueue(npc);
        }

        public void DequeueNpc(INpcController npc)
        {
            if (!_queue.Contains(npc)) return;

            // Queue doesn't support arbitrary removal — rebuild without target
            Queue<INpcController> rebuilt = new Queue<INpcController>();

            foreach (INpcController queued in _queue)
            {
                if (queued != npc)
                    rebuilt.Enqueue(queued);
            }

            _queue.Clear();

            foreach (INpcController queued in rebuilt)
                _queue.Enqueue(queued);
        }

        public bool IsFirstInQueue(INpcController npc)
        {
            if (_queue.Count == 0) return false;
            return _queue.Peek() == npc;
        }

        public void ProcessCheckout(INpcController npc)
        {
            if (!IsFirstInQueue(npc))
            {
                Debug.LogWarning("CheckoutService: ProcessCheckout called on NPC not first in queue.");
                return;
            }

            if (npc.HasItem)
            {
                // NPC pays — money goes to player economy
                // SellPrice used here: NPC sells toy to store (player receives money)
                // ToyData not available at this stage — fixed price as fallback
                _economy.Add(NpcPaymentAmount);
            }

            _queue.Dequeue();
            OnCheckoutCompleted?.Invoke(npc);
        }

        // Fixed payment per NPC transaction
        // Will be replaced with ToyData.SellPrice when item data flows through states
        private const int NpcPaymentAmount = 10;
    }
}