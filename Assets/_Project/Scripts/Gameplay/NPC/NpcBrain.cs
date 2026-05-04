using System.Linq;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    public class NpcBrain : INpcBrain
    {
        private readonly NpcBrainConfig _config;

        public NpcBrain(NpcBrainConfig config)
        {
            _config = config;
        }

        // Decides whether NPC wants to buy based on probability
        // ToyData is optional — decision works without it
        public bool WantsToBuy(ToyData toy)
        {
            return Random.value <= _config.BuyProbability;
        }

        // Selects a random occupied shelf slot to visit
        public IShelfSlot SelectShelfSlot(IShelfSlot[] allSlots)
        {
            if (allSlots == null || allSlots.Length == 0) return null;

            IShelfSlot[] occupiedSlots = allSlots
                .Where(slot => slot.IsOccupied)
                .ToArray();

            if (occupiedSlots.Length == 0) return null;

            return occupiedSlots[Random.Range(0, occupiedSlots.Length)];
        }
    }
}