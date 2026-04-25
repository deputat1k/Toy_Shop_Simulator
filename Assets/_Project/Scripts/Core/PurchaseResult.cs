namespace ToyShop.Core
{
    public readonly struct PurchaseResult
    {
        public string ToyId { get; }
        public bool Success { get; }

        public PurchaseResult(string toyId, bool success)
        {
            ToyId = toyId;
            Success = success;
        }
    }
}