namespace ToyShop.Core.Interfaces
{
    public interface INpcState
    {
        void Enter(INpcController npc);
        void Update(INpcController npc);
        void Exit(INpcController npc);
    }
}