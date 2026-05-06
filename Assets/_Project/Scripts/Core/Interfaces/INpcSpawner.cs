namespace ToyShop.Core.Interfaces
{
    public interface INpcSpawner
    {
        void StartSpawning();
        void StopSpawning();
        int ActiveNpcCount { get; }
    }
}