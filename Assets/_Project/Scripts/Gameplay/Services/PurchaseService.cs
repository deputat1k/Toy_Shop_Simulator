using ToyShop.Core.Interfaces;
using ToyShop.Data;
using Zenject;

namespace ToyShop.Gameplay.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IEconomyService _economy;
        private readonly ICatalogService _catalog;
        private readonly SignalBus _signalBus;

        public PurchaseService(IEconomyService economy, ICatalogService catalog, SignalBus signalBus)
        {
            _economy = economy;
            _catalog = catalog;
            _signalBus = signalBus;
        }

        public bool TryBuyToy(string toyId)
        {
            ToyData toy = _catalog.GetToyById(toyId);

            if (toy == null)
            {
                _signalBus.Fire(new PurchaseResultSignal(toyId, false));
                return false;
            }

            bool success = _economy.TrySpend((int)toy.purchasePrice);

            _signalBus.Fire(new PurchaseResultSignal(toyId, success));
            return success;
        }
    }
}