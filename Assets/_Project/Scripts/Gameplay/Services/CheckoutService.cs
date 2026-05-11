using System;
using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IEconomyService _economy;
        private readonly ICheckoutQueue _checkoutQueue;
        private readonly Queue<INpcController> _queue = new Queue<INpcController>();

        public int QueueLength => _queue.Count;

        public event Action<INpcController> OnCheckoutCompleted;
        public event Action OnQueueChanged;

        public Vector3 GetCounterFacingPosition() => _checkoutQueue.GetPositionAt(0);
        public CheckoutService(IEconomyService economy, ICheckoutQueue checkoutQueue)
        {
            _economy = economy;
            _checkoutQueue = checkoutQueue;
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
            OnQueueChanged?.Invoke();
        }

        public void DequeueNpc(INpcController npc)
        {
            if (!_queue.Contains(npc)) return;

            Queue<INpcController> rebuilt = new Queue<INpcController>();
            foreach (INpcController queued in _queue)
            {
                if (queued != npc)
                    rebuilt.Enqueue(queued);
            }

            _queue.Clear();
            foreach (INpcController queued in rebuilt)
                _queue.Enqueue(queued);

            OnQueueChanged?.Invoke();
        }

        public bool IsFirstInQueue(INpcController npc)
        {
            if (_queue.Count == 0) return false;
            return _queue.Peek() == npc;
        }

        public INpcController GetFirstInQueue()
        {
            if (_queue.Count == 0) return null;
            return _queue.Peek();
        }

        public Vector3 GetNpcQueuePosition(INpcController npc)
        {
            int index = 0;
            foreach (INpcController queued in _queue)
            {
                if (queued == npc)
                    return _checkoutQueue.GetPositionAt(index);
                index++;
            }
            return _checkoutQueue.GetPositionAt(0);
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
                // Use SellPrice from ToyData if available, otherwise fallback
                int payment = npc.SelectedToy != null
                    ? npc.SelectedToy.SellPrice
                    : FallbackPaymentAmount;

                _economy.Add(payment);
            }

            _queue.Dequeue();
            OnCheckoutCompleted?.Invoke(npc);
            OnQueueChanged?.Invoke();
        }

        // Fallback if ToyData is not available on NPC
        private const int FallbackPaymentAmount = 10;
    }
}