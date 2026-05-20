using System;
using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.Gameplay.Pause
{
    public class PauseService : IPauseService
    {
        public bool IsPaused { get; private set; }

        public event Action<bool> OnPauseStateChanged;

        public void Pause()
        {
            if (IsPaused) return;

            Time.timeScale = 0f;
            IsPaused = true;
            OnPauseStateChanged?.Invoke(true);
        }

        public void Resume()
        {
            if (!IsPaused) return;

            Time.timeScale = 1f;
            IsPaused = false;
            OnPauseStateChanged?.Invoke(false);
        }
    }
}