using ToyShop.Core.Interfaces;
using ToyShop.Core.SaveSystem;

namespace ToyShop.Gameplay.SaveSystem
{
    public class PlayerSaveHandler : ISaveHandler
    {
        private readonly IPlayerController _player;

        public PlayerSaveHandler(IPlayerController player)
        {
            _player = player;
        }

        public void OnSave(GameSaveData saveData)
        {
            saveData.PlayerPosition = _player.Transform.position;
        }

        public void OnLoad(GameSaveData saveData)
        {
            _player.SetPosition(saveData.PlayerPosition);
        }
    }
}