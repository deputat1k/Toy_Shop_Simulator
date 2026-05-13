using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.NPC.States
{
    public class WaitInQueueState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _exitState;

        private INpcController _currentNpc;
        private bool _isFacingCounter;

        public WaitInQueueState(NpcContext context, INpcState exitState)
        {
            _context = context;
            _exitState = exitState;
        }

        public void Enter(INpcController npc)
        {
            _currentNpc = npc;
            _isFacingCounter = false;

            _context.CheckoutService.OnCheckoutCompleted += HandleCheckoutCompleted;
            _context.CheckoutService.OnQueueChanged += HandleQueueChanged;
        }

        public void Update(INpcController npc)
        {
            if (!_isFacingCounter)
            {
                // Wait until NPC reaches queue position, then face counter
                if (!npc.HasReachedDestination()) return;

                Vector3 counterPos = _context.CheckoutService.GetCounterFacingPosition();
                npc.SetAgentRotationEnabled(false);
                npc.FaceDirection(counterPos);

                if (npc.IsFacingTarget(counterPos))
                    _isFacingCounter = true;

                return;
            }
        }

        public void Exit(INpcController npc)
        {
            npc.SetAgentRotationEnabled(true);
            _context.CheckoutService.OnCheckoutCompleted -= HandleCheckoutCompleted;
            _context.CheckoutService.OnQueueChanged -= HandleQueueChanged;
            _currentNpc = null;
        }

        private void HandleQueueChanged()
        {
            if (_currentNpc == null) return;

            // Reposition and re-face counter after queue shifts
            _isFacingCounter = false;
            _currentNpc.SetAgentRotationEnabled(true);
            _currentNpc.MoveTo(
                _context.CheckoutService.GetNpcQueuePosition(_currentNpc));
        }

        private void HandleCheckoutCompleted(INpcController completedNpc)
        {
            if (_currentNpc == null) return;
            if (completedNpc != _currentNpc) return;
            _currentNpc.ChangeState(_exitState);
        }
    }
}