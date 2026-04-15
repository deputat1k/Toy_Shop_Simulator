namespace ToyShop.Core.Interfaces
{
    public interface IPurchaseService
    {
        bool TryBuyToy(string toyId);
    }
}