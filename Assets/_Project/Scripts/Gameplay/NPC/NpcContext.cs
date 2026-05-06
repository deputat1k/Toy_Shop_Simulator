using ToyShop.Core.Interfaces;

namespace ToyShop.Gameplay.NPC
{
    // Read-only context passed to each NPC state
    public class NpcContext
    {
        public INpcBrain Brain { get; }
        public ICheckoutService CheckoutService { get; }
        public IPointOfInterestProvider PointsOfInterest { get; }

        public NpcContext(
            INpcBrain brain,
            ICheckoutService checkoutService,
            IPointOfInterestProvider pointsOfInterest)
        {
            Brain = brain;
            CheckoutService = checkoutService;
            PointsOfInterest = pointsOfInterest;
        }
    }
}