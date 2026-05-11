using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    public class NpcAnimator : MonoBehaviour
    {
        // Searches in children — works whether Animator is on root or child mesh
        private Animator _animator;

        private static readonly int SpeedHash =
            Animator.StringToHash("Speed");
        private static readonly int IsInteractingHash =
            Animator.StringToHash("IsInteracting");

        private void Awake()
        {
            // GetComponentInChildren finds Animator on root OR any child
            _animator = GetComponentInChildren<Animator>();

            if (_animator == null)
                Debug.LogError("NpcAnimator: Animator component not found on NPC or its children.");
        }

        public void SetSpeed(float speed)
        {
            if (_animator == null) return;
            _animator.SetFloat(SpeedHash, speed);
        }

        public void SetMoving(bool isMoving) { }

        public void PlayInteractAnimation()
        {
            if (_animator == null) return;
            _animator.SetBool(IsInteractingHash, true);
        }

        public void StopInteractAnimation()
        {
            if (_animator == null) return;
            _animator.SetBool(IsInteractingHash, false);
        }
    }
}