using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.NPC.States
{
    public class ExitStoreState : INpcState
    {
        private readonly NpcContext _context;
        private bool _returned;

        public ExitStoreState(NpcContext context)
        {
            _context = context;
        }

        public void Enter(INpcController npc)
        {
            _returned = false;
            npc.HasItem = false;
            npc.HideItemVisual();
            npc.SelectedToy = null;
            npc.TargetSlot = null;
            npc.MoveTo(_context.PointsOfInterest.GetExitPoint());
        }

        public void Update(INpcController npc)
        {
            if (_returned) return;
            if (!npc.HasReachedDestination()) return;

            _returned = true;
            npc.NotifyReadyToReturn();
        }

        public void Exit(INpcController npc) { }
    }
}