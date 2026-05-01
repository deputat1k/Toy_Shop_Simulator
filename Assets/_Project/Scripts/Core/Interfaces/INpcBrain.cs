using ToyShop.Data;

namespace ToyShop.Core.Interfaces
{
    public interface INpcBrain
    {
        // Decides whether NPC wants to buy based on ToyData
        bool WantsToBuy(ToyData toy);

        // Selects a shelf slot to visit from available occupied slots
        IShelfSlot SelectShelfSlot(IShelfSlot[] allSlots);
    }
}