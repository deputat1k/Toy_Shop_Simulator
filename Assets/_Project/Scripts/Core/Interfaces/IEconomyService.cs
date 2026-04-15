namespace ToyShop.Core.Interfaces
{
    public interface IEconomyService
    {
        int CurrentBalance { get; }
        bool TrySpend(int amount);
    }
}