using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToyShop.UI.HUD
{
    public class CurrencyView : MonoBehaviour
    {
        [Header("Balance Display")]
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private Image _panelBackground;

        [Header("Popup")]
        [SerializeField] private RectTransform _popupAnchor;
        [SerializeField] private float _popupRiseDistance = 65f;
        [SerializeField] private float _popupDuration = 1.3f;

        [Header("Colors")]
        [SerializeField] private Color _normalTextColor = new Color(0.96f, 0.78f, 0.17f); // gold
        [SerializeField] private Color _earnColor = new Color(0.18f, 0.83f, 0.34f); // green
        [SerializeField] private Color _spendColor = new Color(0.91f, 0.25f, 0.25f); // red

        [Header("Flash")]
        [SerializeField] private float _flashHoldDuration = 0.12f;
        [SerializeField] private float _flashFadeDuration = 0.38f;

        [Header("Audio — Optional")]
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _earnSound;
        [SerializeField] private AudioClip _spendSound;

        private Coroutine _flashRoutine;

        private void Awake()
        {
            if (_balanceText != null)
                _balanceText.color = _normalTextColor;
        }

        // Called by CurrencyPresenter on every balance change
        // delta == 0 means initial display — no popup, no flash
        public void UpdateBalance(int amount, int delta)
        {
            if (_balanceText != null)
                _balanceText.text = $"{amount:N0}";

            if (delta == 0) return;

            bool isEarning = delta > 0;
            Color feedbackColor = isEarning ? _earnColor : _spendColor;

            FlashBalanceText(feedbackColor);
            SpawnPopup(delta, feedbackColor);
            PlayFeedbackSound(isEarning);
        }

        // Flash 

        private void FlashBalanceText(Color flashColor)
        {
            if (_balanceText == null) return;
            if (_flashRoutine != null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(FlashRoutine(flashColor));
        }

        private IEnumerator FlashRoutine(Color flashColor)
        {
            _balanceText.color = flashColor;
            yield return new WaitForSecondsRealtime(_flashHoldDuration);

            float elapsed = 0f;
            while (elapsed < _flashFadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / _flashFadeDuration;
                _balanceText.color = Color.Lerp(flashColor, _normalTextColor, t);
                yield return null;
            }

            _balanceText.color = _normalTextColor;
        }

        // Popup 

        private void SpawnPopup(int delta, Color color)
        {
            if (_popupAnchor == null) return;

            string text = delta > 0
                ? $"+${delta:N0}"
                : $"-${Mathf.Abs(delta):N0}";

            var go = BuildPopupObject(text, color);
            StartCoroutine(AnimatePopup(go));
        }

        private GameObject BuildPopupObject(string text, Color color)
        {
            var go = new GameObject("SalePopup");

            // Adding TMP first causes Unity to auto-add RectTransform
            var cg = go.AddComponent<CanvasGroup>();
            var tmp = go.AddComponent<TextMeshProUGUI>();

            go.transform.SetParent(_popupAnchor, worldPositionStays: false);

            var rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = new Vector2(180f, 44f);

            tmp.text = text;
            tmp.color = color;
            tmp.fontSize = 28;
            tmp.fontStyle = FontStyles.Bold;
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.enableAutoSizing = false;

            cg.alpha = 1f;

            return go;
        }

        private IEnumerator AnimatePopup(GameObject go)
        {
            if (go == null) yield break;

            var rect = go.GetComponent<RectTransform>();
            var cg = go.GetComponent<CanvasGroup>();
            Vector2 start = rect.anchoredPosition;
            Vector2 end = start + Vector2.up * _popupRiseDistance;
            float fadeStartRatio = 0.4f; // fade starts at 40% of animation

            float elapsed = 0f;
            while (elapsed < _popupDuration)
            {
                if (go == null) yield break;

                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / _popupDuration;

                rect.anchoredPosition = Vector2.Lerp(start, end, Mathf.SmoothStep(0f, 1f, t));

                if (t > fadeStartRatio)
                {
                    float fadeT = (t - fadeStartRatio) / (1f - fadeStartRatio);
                    cg.alpha = Mathf.Lerp(1f, 0f, fadeT);
                }

                yield return null;
            }

            if (go != null) Destroy(go);
        }

        // Audio 

        private void PlayFeedbackSound(bool isEarning)
        {
            if (_audioSource == null) return;

            AudioClip clip = isEarning ? _earnSound : _spendSound;
            if (clip != null)
                _audioSource.PlayOneShot(clip);
        }

        // Validation

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_balanceText == null)
                Debug.LogWarning("CurrencyView: Balance Text not assigned.");
            if (_popupAnchor == null)
                Debug.LogWarning("CurrencyView: Popup Anchor not assigned — sale popups won't appear.");
        }
#endif
    }
}