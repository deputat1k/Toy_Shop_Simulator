using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.NPC.States
{
    public class IdleAtShelfState : INpcState
    {
        private readonly NpcContext _context;
        private readonly INpcState _selectItemState;
        private readonly float _idleDuration;

        private enum Phase { Rotating, Waiting }

        private Phase _phase;
        private float _timer;
        private Vector3 _shelfPosition;
        private bool _slotValid;

        public IdleAtShelfState(NpcContext context, INpcState selectItemState, float idleDuration = 2f)
        {
            _context = context;
            _selectItemState = selectItemState;
            _idleDuration = idleDuration;
        }

        public void Enter(INpcController npc)
        {
            _timer = 0f;

            if (npc.TargetSlot == null || !npc.TargetSlot.IsOccupied)
            {
                _slotValid = false;
                npc.ChangeState(_selectItemState);
                return;
            }

            _slotValid = true;
            _shelfPosition = npc.TargetSlot.SlotTransform.position;
            _phase = Phase.Rotating;
            npc.SetAgentRotationEnabled(false);
        }

        public void Update(INpcController npc)
        {
            if (!_slotValid) return;

            if (_phase == Phase.Rotating)
            {
                npc.FaceDirection(_shelfPosition);

                if (npc.IsFacingTarget(_shelfPosition))
                    _phase = Phase.Waiting;

                return;
            }

            // Just wait — no animation here
            // Animation plays in SelectItemState only if NPC actually buys
            _timer += Time.deltaTime;
            if (_timer >= _idleDuration)
                npc.ChangeState(_selectItemState);
        }

        public void Exit(INpcController npc)
        {
            npc.SetAgentRotationEnabled(true);
        }
    }
}