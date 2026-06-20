using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    public class NpcAnimator : MonoBehaviour
    {
        [Header("Walk Sync")]
        [Tooltip("Швидкість руху при якій Mixamo Walk кліп виглядає природно. Підбирається на око.")]
        [SerializeField] private float _referenceWalkSpeed = 1.4f;
        [SerializeField] private float _minPlaybackRate = 0.6f;
        [SerializeField] private float _maxPlaybackRate = 1.4f;

        private Animator _animator;

        private static readonly int SpeedHash =
            Animator.StringToHash("Speed");
        private static readonly int IsInteractingHash =
            Animator.StringToHash("IsInteracting");

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();

            if (_animator == null)
                Debug.LogError("NpcAnimator: Animator component not found on NPC or its children.");
        }

        public void SetSpeed(float speed)
        {
            if (_animator == null) return;

            _animator.SetFloat(SpeedHash, speed);

            // Масштабуємо швидкість відтворення анімації під реальну швидкість руху —
            // прибирає ковзання ніг під час розгону/гальмування
            if (speed > 0.05f && _referenceWalkSpeed > 0f)
            {
                float rate = speed / _referenceWalkSpeed;
                _animator.speed = Mathf.Clamp(rate, _minPlaybackRate, _maxPlaybackRate);
            }
            else
            {
                _animator.speed = 1f; // idle / interact завжди в нормальному темпі
            }
        }

        public void SetMoving(bool isMoving) { }

        public void PlayInteractAnimation()
        {
            if (_animator == null) return;
            _animator.speed = 1f; // інтеракшн-анімація завжди в нормальному темпі
            _animator.SetBool(IsInteractingHash, true);
        }

        public void StopInteractAnimation()
        {
            if (_animator == null) return;
            _animator.SetBool(IsInteractingHash, false);
        }
    }
}