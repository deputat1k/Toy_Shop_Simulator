using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IDeliveryPointProvider
    {
        Vector3 GetSpawnPosition();
    }
}