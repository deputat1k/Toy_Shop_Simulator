using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IPointOfInterestProvider
    {
        // Entry point where NPCs spawn and start their path
        Vector3 GetEntryPoint();

        // Exit point where NPCs leave after checkout
        Vector3 GetExitPoint();

        // Checkout counter position for queue
        Vector3 GetCheckoutPoint();

        // All shelf slots available for browsing
        IShelfSlot[] GetAllShelfSlots();
    }
}