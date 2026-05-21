using UnityEngine;

namespace ToyShop.Core.Interfaces
{
    public interface IHudNotificationService
    {
        void ShowMessage(string message, Color color, float duration = 2f);
    }
}