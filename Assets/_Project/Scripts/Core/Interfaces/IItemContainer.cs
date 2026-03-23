using System;

namespace ToyShop.Core.Interfaces
{
    public interface IItemContainer
    {
       
        bool CanExtract { get; }

        //Attempt to retrieve an item. Returns true if successful.
        bool TryExtract(out IItemGrabbable extractedItem);

      
        event Action OnItemExtracted;
        event Action OnContainerEmpty;
    }
}
