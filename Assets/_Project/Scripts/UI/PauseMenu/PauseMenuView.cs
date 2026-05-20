using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ToyShop.UI.PauseMenu
{
    public class PauseMenuView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _loadButton;

        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private float _notificationDuration = 2f;

        public event Action OnResumeClicked;
        public event Action OnSaveClicked;
        public event Action OnLoadClicked;

        private Coroutine _notificationRoutine;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(() => OnResumeClicked?.Invoke());
            _saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
            _loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());

            HideNotification();
            Hide();
        }

        public void Show() => gameObject.SetActive(true);

        public void Hide()
        {
            HideNotification();
            gameObject.SetActive(false);
        }

        public void ShowNotification(string message, Color color)
        {
            if (_notificationText == null) return;

            _notificationText.text = message;
            _notificationText.color = color;
            _notificationText.gameObject.SetActive(true);

            if (_notificationRoutine != null)
                StopCoroutine(_notificationRoutine);

            _notificationRoutine = StartCoroutine(HideNotificationAfterDelay());
        }

        private void HideNotification()
        {
            if (_notificationText != null)
                _notificationText.gameObject.SetActive(false);
        }

        private IEnumerator HideNotificationAfterDelay()
        {
            // WaitForSecondsRealtime works when Time.timeScale = 0 (game is paused)
            yield return new WaitForSecondsRealtime(_notificationDuration);
            HideNotification();
        }

        private void OnDestroy()
        {
            if (_resumeButton != null) _resumeButton.onClick.RemoveAllListeners();
            if (_saveButton != null) _saveButton.onClick.RemoveAllListeners();
            if (_loadButton != null) _loadButton.onClick.RemoveAllListeners();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_resumeButton == null)
                Debug.LogWarning("PauseMenuView: Resume Button not assigned.");
            if (_saveButton == null)
                Debug.LogWarning("PauseMenuView: Save Button not assigned.");
            if (_loadButton == null)
                Debug.LogWarning("PauseMenuView: Load Button not assigned.");
        }
#endif
    }
}