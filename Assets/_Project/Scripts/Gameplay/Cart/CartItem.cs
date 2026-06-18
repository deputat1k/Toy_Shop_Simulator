using ToyShop.Data;

namespace ToyShop.Gameplay.Cart
{
    public class CartItem
    {
        public ToyData ToyData { get; }
        public int Quantity { get; set; }

        // Uses PurchasePrice — what the player pays, not SellPrice (what NPCs pay)
        public int LineTotal => ToyData.PurchasePrice * Quantity;

        public CartItem(ToyData toyData, int quantity = 1)
        {
            ToyData = toyData;
            Quantity = quantity;
        }
    }
}