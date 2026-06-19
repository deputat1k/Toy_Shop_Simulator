using ToyShop.Core.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ToyShop.Infrastructure
{
    public class SceneLoader : ISceneLoader
    {
        public const string MainMenuScene = "MainMenu";
        public const string GameplayScene = "SampleScene";

        public void LoadScene(string sceneName)
        {
            // Reset time before transition — covers cases where Resume() wasn't called
            Time.timeScale = 1f;

            // Reset cursor before transition — receiving scene sets its own state
            // Gameplay scene: CursorController.Initialize() corrects it immediately
            // Menu scenes: cursor stays visible (correct for UI interaction)
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene(sceneName);
        }
    }
}