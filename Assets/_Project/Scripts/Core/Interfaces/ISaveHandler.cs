using ToyShop.Core.SaveSystem;

namespace ToyShop.Core.Interfaces
{
    // Implement on any system that participates in save/load
    public interface ISaveHandler
    {
        void OnSave(GameSaveData saveData);
        void OnLoad(GameSaveData saveData);
    }
}