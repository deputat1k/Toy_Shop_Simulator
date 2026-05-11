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

            // Move to assigned queue position based on current index
            npc.MoveTo(_context.CheckoutService.GetNpcQueuePosition(npc));
        }

        public void Update(INpcController npc)
        {
            if (npc.HasReachedDestination())
                npc.ChangeState(_waitInQueueState);
        }

        public void Exit(INpcController npc) { }
    }
}