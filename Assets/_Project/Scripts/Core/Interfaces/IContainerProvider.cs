namespace ToyShop.Core.Interfaces
{
    public interface IContainerProvider
    {
        bool TryGetContainer(out IItemContainer container);
    }
}