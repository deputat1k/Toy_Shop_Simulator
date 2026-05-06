using System;
using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.NPC.States
{
    public class WaitInQueueState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _exitState;

        private INpcController _currentNpc;
        private bool _isProcessing;

        public WaitInQueueState(NpcContext context, INpcState exitState)
        {
            _context = context;
            _exitState = exitState;
        }

        public void Enter(INpcController npc)
        {
            _currentNpc = npc;
            _isProcessing = false;
            _context.CheckoutService.OnCheckoutCompleted += HandleCheckoutCompleted;
        }

        public void Update(INpcController npc)
        {
            if (_isProcessing) return;
            if (!_context.CheckoutService.IsFirstInQueue(npc)) return;

            _isProcessing = true;
            _context.CheckoutService.ProcessCheckout(npc);
        }

        public void Exit(INpcController npc)
        {
            _context.CheckoutService.OnCheckoutCompleted -= HandleCheckoutCompleted;
            _currentNpc = null;
        }

        private void HandleCheckoutCompleted(INpcController completedNpc)
        {
            if (_currentNpc == null) return;
            if (completedNpc != _currentNpc) return;

            _currentNpc.ChangeState(_exitState);
        }
    }
}