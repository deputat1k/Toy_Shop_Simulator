using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.NPC.States
{
    public class EnterStoreState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _nextState;

        public EnterStoreState(NpcContext context, INpcState nextState)
        {
            _context = context;
            _nextState = nextState;
        }

        public void Enter(INpcController npc)
        {
            npc.MoveTo(_context.PointsOfInterest.GetEntryPoint());
        }

        public void Update(INpcController npc)
        {
            if (npc.HasReachedDestination())
                npc.ChangeState(_nextState);
        }

        public void Exit(INpcController npc) { }
    }
}