using System;

namespace ToyShop.Core.Interfaces
{
    public interface IGameStateService
    {
        bool IsTabletOpen { get; }
        event Action<bool> OnTabletStateChanged;
    }
}