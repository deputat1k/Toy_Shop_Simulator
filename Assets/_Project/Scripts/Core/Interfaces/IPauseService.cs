using System;

namespace ToyShop.Core.Interfaces
{
    public interface IPauseService
    {
        bool IsPaused { get; }
        void Pause();
        void Resume();
        event Action<bool> OnPauseStateChanged;
    }
}