using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IPointOfInterestProvider
    {
        Vector3 GetEntryPoint();
        Vector3 GetExitPoint();
        IShelfSlot[] GetAllShelfSlots();
    }
}