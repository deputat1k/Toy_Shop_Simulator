using System.Collections.Generic;
using System.Linq;
using ToyShop.Core.Interfaces;
using ToyShop.Data;

namespace ToyShop.Gameplay.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ToyDatabase _database;

        public CatalogService(ToyDatabase database)
        {
            _database = database;
        }

        public IReadOnlyList<ToyData> GetAllToys() => _database.Toys;

 
           public ToyData GetToyById(string id) => _database.Toys.FirstOrDefault(t => t.Id == id);
    }
}