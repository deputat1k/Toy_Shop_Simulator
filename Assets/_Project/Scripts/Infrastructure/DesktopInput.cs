using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Infrastructure
{
    public class DesktopInput : IInputProvider
    {
        public Vector2 GetMovementDirection()
        {
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (direction.sqrMagnitude > 1f) direction.Normalize(); // sqrMagnitude ןנאצ‏÷ רגטהרו
            return direction;
        }

        public bool IsInteractActionTriggered() => Input.GetKeyDown(KeyCode.E);
    }
}