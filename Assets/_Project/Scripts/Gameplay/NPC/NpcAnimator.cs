using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    // Stub for future animation integration
    // Add Animator component and animator controller when animations are ready
    public class NpcAnimator : MonoBehaviour
    {
        // private Animator _animator;
        // private static readonly int SpeedHash = Animator.StringToHash("Speed");
        // private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

        // private void Awake() => _animator = GetComponent<Animator>();

        public void SetMoving(bool isMoving)
        {
            // _animator.SetBool(IsWalkingHash, isMoving);
        }

        public void SetSpeed(float speed)
        {
            // _animator.SetFloat(SpeedHash, speed);
        }

        public void PlayPickupAnimation()
        {
            // _animator.SetTrigger("Pickup");
        }
    }
}