using System.Collections.Generic;
using ToyShop.Data;

namespace ToyShop.Core.Interfaces
{
    public interface ICatalogService
    {
        IReadOnlyList<ToyData> GetAllToys();
        ToyData GetToyById(string id);
    }
}