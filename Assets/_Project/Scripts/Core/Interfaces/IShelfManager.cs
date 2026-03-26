using System;

namespace ToyShop.Core.Interfaces
{
    public interface IShelfManager
    {
        event Action OnShelfFull;
        event Action OnEmptyContainerProvided;
        void ProcessInteraction(IItemHolder holder);
    }
}