using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.NPC.States
{
    public class MoveToCheckoutState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _waitInQueueState;

        public MoveToCheckoutState(NpcContext context, INpcState waitInQueueState)
        {
            _context = context;
            _waitInQueueState = waitInQueueState;
        }

        public void Enter(INpcController npc)
        {
            _context.CheckoutService.EnqueueNpc(npc);
            npc.MoveTo(_context.PointsOfInterest.GetCheckoutPoint());
        }

        public void Update(INpcController npc)
        {
            if (npc.HasReachedDestination())
                npc.ChangeState(_waitInQueueState);
        }

        public void Exit(INpcController npc) { }
    }
}