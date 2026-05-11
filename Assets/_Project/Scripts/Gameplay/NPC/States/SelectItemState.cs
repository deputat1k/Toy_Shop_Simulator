using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.NPC.States
{
    public class SelectItemState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _checkoutState;
        private readonly INpcState _exitState;
        private readonly float _postPickupDelay;

        private bool _willProceed;
        private float _timer;

        public SelectItemState(
            NpcContext context,
            INpcState checkoutState,
            INpcState exitState,
            float postPickupDelay = 1f)
        {
            _context = context;
            _checkoutState = checkoutState;
            _exitState = exitState;
            _postPickupDelay = postPickupDelay;
        }

        public void Enter(INpcController npc)
        {
            _timer = 0f;
            _willProceed = false;

            IShelfSlot slot = npc.TargetSlot;

            if (slot == null || !slot.IsOccupied)
            {
                npc.ChangeState(_exitState);
                return;
            }

            if (!_context.Brain.WantsToBuy(null))
            {
                npc.HasItem = false;
                npc.ChangeState(_exitState);
                return;
            }

            // NPC decided to buy — play animation and take item
            IPlaceable item = slot.CurrentItem;
            if (item is IToyDataHolder holder)
                npc.SelectedToy = holder.ToyData;

            slot.Free();
            if (item is MonoBehaviour mb)
                Object.Destroy(mb.gameObject);

            npc.HasItem = true;
            npc.ShowItemVisual();
            npc.PlayInteractAnimation(); // only here — only when actually buying
            _willProceed = true;
        }

        public void Update(INpcController npc)
        {
            if (!_willProceed) return;

            _timer += Time.deltaTime;
            if (_timer >= _postPickupDelay)
            {
                npc.StopInteractAnimation();
                npc.ChangeState(_checkoutState);
            }
        }

        public void Exit(INpcController npc) { }
    }
}