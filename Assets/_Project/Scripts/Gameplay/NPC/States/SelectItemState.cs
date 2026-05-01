using ToyShop.Core.Interfaces;
using ToyShop.Data;

namespace ToyShop.Gameplay.NPC.States
{
    public class SelectItemState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _checkoutState;
        private readonly INpcState _exitState;

        public SelectItemState(
            NpcContext context,
            INpcState checkoutState,
            INpcState exitState)
        {
            _context = context;
            _checkoutState = checkoutState;
            _exitState = exitState;
        }

        public void Enter(INpcController npc)
        {
            IShelfSlot slot = npc.TargetSlot;

            if (slot == null || !slot.IsOccupied)
            {
                // Slot became empty before NPC arrived
                npc.ChangeState(_exitState);
                return;
            }

            // Slot holds an IPlaceable — we need ToyData from it
            // for now Brain decides purely on probability
            bool wants = _context.Brain.WantsToBuy(null);

            if (wants)
            {
                npc.HasItem = true;
                npc.ChangeState(_checkoutState);
            }
            else
            {
                npc.HasItem = false;
                npc.ChangeState(_exitState);
            }
        }

        public void Update(INpcController npc) { }

        public void Exit(INpcController npc) { }
    }
}