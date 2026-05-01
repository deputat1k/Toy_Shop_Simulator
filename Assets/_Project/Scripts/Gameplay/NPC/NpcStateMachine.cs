using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    public class NpcStateMachine : INpcStateMachine
    {
        public INpcState CurrentState { get; private set; }

        public void ChangeState(INpcState newState, INpcController npc)
        {
            if (newState == null)
            {
                Debug.LogError("NpcStateMachine: attempted to change to null state.");
                return;
            }

            CurrentState?.Exit(npc);

            CurrentState = newState;

            CurrentState.Enter(npc);
        }
    }
}