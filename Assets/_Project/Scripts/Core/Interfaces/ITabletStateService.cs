using System;

namespace ToyShop.Core.Interfaces
{
    public interface ITabletStateService
    {
        bool IsTabletOpen { get; }

        // Explicit close — used by ESC handler when tablet is open
        void Close();

        event Action<bool> OnTabletStateChanged;
    }
}