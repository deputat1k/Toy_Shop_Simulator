using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface ICheckoutQueue
    {
        int Capacity { get; }
        Vector3 GetPositionAt(int index);

        // Dedicated point all NPCs face toward while waiting
   
    }
}