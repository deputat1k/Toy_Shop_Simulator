using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.NPC.States
{
    public class BrowseShelfState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _selectItemState;
        private readonly INpcState _exitState;

        public BrowseShelfState(
            NpcContext context,
            INpcState selectItemState,
            INpcState exitState)
        {
            _context = context;
            _selectItemState = selectItemState;
            _exitState = exitState;
        }

        public void Enter(INpcController npc)
        {
            IShelfSlot[] allSlots = _context.PointsOfInterest.GetAllShelfSlots();
            IShelfSlot target = _context.Brain.SelectShelfSlot(allSlots);

            if (target == null)
            {
                npc.ChangeState(_exitState);
                return;
            }

            npc.TargetSlot = target;
            npc.MoveTo(target.SlotTransform.position);
        }

        public void Update(INpcController npc)
        {
            if (npc.HasReachedDestination())
                npc.ChangeState(_selectItemState);
        }

        public void Exit(INpcController npc) { }
    }
}