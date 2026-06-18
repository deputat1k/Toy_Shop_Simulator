using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ToyShop.UI.Common
{
    // Attach to any Button for scale-based hover/press feedback
    // Works alongside Sprite Swap transition on the Button component
    [RequireComponent(typeof(RectTransform))]
    public class ButtonHoverEffect : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float _hoverScale = 1.05f;
        [SerializeField] private float _pressScale = 0.96f;
        [SerializeField] private float _lerpSpeed = 14f;

        private RectTransform _rect;
        private Coroutine _scaleRoutine;

        private void Awake() => _rect = GetComponent<RectTransform>();

        public void OnPointerEnter(PointerEventData e) => AnimateTo(_hoverScale);
        public void OnPointerExit(PointerEventData e) => AnimateTo(1f);
        public void OnPointerDown(PointerEventData e) => AnimateTo(_pressScale);
        public void OnPointerUp(PointerEventData e) => AnimateTo(_hoverScale);

        private void AnimateTo(float target)
        {
            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _scaleRoutine = StartCoroutine(ScaleRoutine(target));
        }

        private IEnumerator ScaleRoutine(float target)
        {
            while (!Mathf.Approximately(_rect.localScale.x, target))
            {
                float current = _rect.localScale.x;
                float next = Mathf.Lerp(current, target, Time.unscaledDeltaTime * _lerpSpeed);
                _rect.localScale = Vector3.one * next;
                yield return null;
            }

            _rect.localScale = Vector3.one * target;
        }

        private void OnDisable()
        {
            if (_scaleRoutine != null) StopCoroutine(_scaleRoutine);
            _rect.localScale = Vector3.one;
        }
    }
}