using System.Collections;
using UnityEngine;

namespace ToyShop.UI.Common
{
    // Reusable coroutine-based UI animations — all use unscaled time
    public static class UIAnimator
    {
        public static IEnumerator FadeIn(CanvasGroup group, float duration = 0.3f)
        {
            group.alpha = 0f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                group.alpha = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                yield return null;
            }
            group.alpha = 1f;
        }

        public static IEnumerator FadeOut(CanvasGroup group, float duration = 0.25f)
        {
            group.alpha = 1f;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                group.alpha = 1f - Mathf.SmoothStep(0f, 1f, elapsed / duration);
                yield return null;
            }
            group.alpha = 0f;
        }

        public static IEnumerator ScaleIn(RectTransform rect, float duration = 0.25f)
        {
            Vector3 from = Vector3.one * 0.85f;
            rect.localScale = from;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                rect.localScale = Vector3.LerpUnclamped(from, Vector3.one, t);
                yield return null;
            }
            rect.localScale = Vector3.one;
        }

        // Slides rect FROM (its position + startOffset) TO its current position
        public static IEnumerator SlideIn(RectTransform rect, Vector2 startOffset, float duration = 0.3f)
        {
            Vector2 targetPos = rect.anchoredPosition;
            rect.anchoredPosition = targetPos + startOffset;

            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                rect.anchoredPosition = Vector2.Lerp(targetPos + startOffset, targetPos, t);
                yield return null;
            }
            rect.anchoredPosition = targetPos;
        }

        // Slides rect FROM its current position TO (its position + endOffset), then resets
        public static IEnumerator SlideOut(RectTransform rect, Vector2 endOffset, float duration = 0.25f)
        {
            Vector2 startPos = rect.anchoredPosition;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
                rect.anchoredPosition = Vector2.Lerp(startPos, startPos + endOffset, t);
                yield return null;
            }
            rect.anchoredPosition = startPos; // reset for next open
        }
    }
}