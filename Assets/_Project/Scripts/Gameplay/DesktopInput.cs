using UnityEngine;

namespace ToyShop.Gameplay
{
    public class DesktopInput : MonoBehaviour, IInputProvider
    {
        public Vector2 GetMovementDirection()
        {
            // „итаЇмо дан≥ з клав≥атури
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            Vector2 direction = new Vector2(x, y);
            if (direction.magnitude > 1f)
            {
                direction.Normalize();
            }

            return direction;
        }
    }
}