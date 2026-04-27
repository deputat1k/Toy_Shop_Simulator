using System;

namespace ToyShop.Core.Interfaces
{
    public interface ITabletStateService
    {
        bool IsTabletOpen { get; }
        event Action<bool> OnTabletStateChanged;
    }
}