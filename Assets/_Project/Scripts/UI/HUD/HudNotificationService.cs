using ToyShop.Core.Interfaces;
using UnityEngine;

namespace ToyShop.UI.HUD
{
    public class HudNotificationService : IHudNotificationService
    {
        private readonly HudNotificationView _view;

        public HudNotificationService(HudNotificationView view)
        {
            _view = view;
        }

        public void ShowMessage(string message, Color color, float duration = 2f)
        {
            _view.Show(message, color, duration);
        }
    }
}