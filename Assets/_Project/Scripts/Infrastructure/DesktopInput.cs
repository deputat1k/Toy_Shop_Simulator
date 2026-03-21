using UnityEngine;
using ToyShop.Core.Interfaces;

namespace ToyShop.Infrastructure
{
    public class DesktopInput : IInputProvider
    {
        public Vector2 GetMovementDirection()
        {
            Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            if (direction.sqrMagnitude > 1f) direction.Normalize(); // sqrMagnitude працює швидше
            return direction;
        }

        public bool IsInteractActionTriggered() => Input.GetKeyDown(KeyCode.E);
        public bool IsThrowActionTriggered()
        {
            // 0 - це ліва кнопка миші
            return Input.GetMouseButtonDown(0);
        }
    }
}