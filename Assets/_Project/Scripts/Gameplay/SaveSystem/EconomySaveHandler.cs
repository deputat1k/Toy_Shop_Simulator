using ToyShop.Core.Interfaces;
using ToyShop.Core.SaveSystem;

namespace ToyShop.Gameplay.SaveSystem
{
    public class EconomySaveHandler : ISaveHandler
    {
        private readonly IEconomyService _economy;

        public EconomySaveHandler(IEconomyService economy)
        {
            _economy = economy;
        }

        public void OnSave(GameSaveData saveData)
        {
            saveData.PlayerBalance = _economy.CurrentBalance;
        }

        public void OnLoad(GameSaveData saveData)
        {
            _economy.SetBalance(saveData.PlayerBalance);
        }
    }
}