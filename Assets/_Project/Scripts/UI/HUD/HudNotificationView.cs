using System.Collections;
using TMPro;
using UnityEngine;

namespace ToyShop.UI.HUD
{
    public class HudNotificationView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _messageText;

        private Coroutine _hideRoutine;

        private void Awake() => HideMessage();

        public void Show(string message, Color color, float duration)
        {
            if (_messageText == null) return;

            _messageText.text = message;
            _messageText.color = color;
            _messageText.gameObject.SetActive(true);

            if (_hideRoutine != null)
                StopCoroutine(_hideRoutine);

            _hideRoutine = StartCoroutine(HideAfterDelay(duration));
        }

        private void HideMessage()
        {
            if (_messageText != null)
                _messageText.gameObject.SetActive(false);
        }

        private IEnumerator HideAfterDelay(float duration)
        {
            // WaitForSecondsRealtime — works when Time.timeScale = 0
            yield return new WaitForSecondsRealtime(duration);
            HideMessage();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_messageText == null)
                Debug.LogWarning("HudNotificationView: Message Text not assigned.");
        }
#endif
    }
}