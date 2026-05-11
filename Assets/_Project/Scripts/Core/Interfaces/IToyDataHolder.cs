using ToyShop.Data;

namespace ToyShop.Core.Interfaces
{
    public interface IToyDataHolder
    {
        ToyData ToyData { get; }
        void SetToyData(ToyData data);
    }
}