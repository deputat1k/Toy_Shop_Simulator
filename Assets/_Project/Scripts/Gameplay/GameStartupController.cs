using ToyShop.Core.Interfaces;
using UnityEngine;
using Zenject;

namespace ToyShop.Gameplay
{
    // Checks on scene load whether player came from "Load Game" in main menu
    // If so, auto-loads the save file
    public class GameStartupController : IInitializable
    {
        public const string AutoLoadKey = "AutoLoadSave";

        private readonly ISaveService _saveService;

        public GameStartupController(ISaveService saveService)
        {
            _saveService = saveService;
        }

        public void Initialize()
        {
            if (PlayerPrefs.GetInt(AutoLoadKey, 0) != 1) return;

            PlayerPrefs.DeleteKey(AutoLoadKey);

            if (_saveService.HasSave)
                _saveService.Load();
        }
    }
}