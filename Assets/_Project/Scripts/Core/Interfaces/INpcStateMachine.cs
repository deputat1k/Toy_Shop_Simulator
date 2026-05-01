namespace ToyShop.Core.Interfaces
{
    public interface INpcStateMachine
    {
        INpcState CurrentState { get; }
        void ChangeState(INpcState newState, INpcController npc);
    }
}