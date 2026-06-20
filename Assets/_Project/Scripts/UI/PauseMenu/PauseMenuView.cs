using System;
using System.Collections;
using TMPro;
using ToyShop.UI.Common;
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
        [SerializeField] private Button _mainMenuButton;

        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private float _notificationDuration = 2f;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _menuPanel;

        public event Action OnResumeClicked;
        public event Action OnSaveClicked;
        public event Action OnLoadClicked;
        public event Action OnMainMenuClicked;

        private Coroutine _notificationRoutine;
        private Coroutine _hideRoutine;

        private void Awake()
        {
            _resumeButton.onClick.AddListener(() => OnResumeClicked?.Invoke());
            _saveButton.onClick.AddListener(() => OnSaveClicked?.Invoke());
            _loadButton.onClick.AddListener(() => OnLoadClicked?.Invoke());
            _mainMenuButton.onClick.AddListener(() => OnMainMenuClicked?.Invoke());

            HideNotification();

            // Set initial hidden state directly — no animation needed at Awake
            if (_canvasGroup != null) _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            // Cancel pending hide if Show called before hide finishes
            if (_hideRoutine != null)
            {
                StopCoroutine(_hideRoutine);
                _hideRoutine = null;
            }

            gameObject.SetActive(true);
            StartCoroutine(PlayShowAnimation());
        }

        public void Hide()
        {
            _hideRoutine = StartCoroutine(PlayHideAnimation());
        }

        private IEnumerator PlayShowAnimation()
        {
            StartCoroutine(UIAnimator.FadeIn(_canvasGroup, 0.25f));
            yield return StartCoroutine(UIAnimator.ScaleIn(_menuPanel, 0.2f));
        }

        private IEnumerator PlayHideAnimation()
        {
            yield return StartCoroutine(UIAnimator.FadeOut(_canvasGroup, 0.2f));
            HideNotification();
            gameObject.SetActive(false);
            _hideRoutine = null;
        }

        public void ShowNotification(string message, Color color)
        {
            if (_notificationText == null) return;

            _notificationText.text = message;
            _notificationText.color = color;
            _notificationText.gameObject.SetActive(true);

            if (_notificationRoutine != null) StopCoroutine(_notificationRoutine);
            _notificationRoutine = StartCoroutine(HideNotificationAfterDelay());
        }

        private void HideNotification()
        {
            if (_notificationText != null)
                _notificationText.gameObject.SetActive(false);
        }

        private IEnumerator HideNotificationAfterDelay()
        {
            yield return new WaitForSecondsRealtime(_notificationDuration);
            HideNotification();
        }

        private void OnDestroy()
        {
            if (_resumeButton != null) _resumeButton.onClick.RemoveAllListeners();
            if (_saveButton != null) _saveButton.onClick.RemoveAllListeners();
            if (_loadButton != null) _loadButton.onClick.RemoveAllListeners();
            if (_mainMenuButton != null) _mainMenuButton.onClick.RemoveAllListeners();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_resumeButton == null) Debug.LogWarning("PauseMenuView: Resume Button not assigned.");
            if (_saveButton == null) Debug.LogWarning("PauseMenuView: Save Button not assigned.");
            if (_loadButton == null) Debug.LogWarning("PauseMenuView: Load Button not assigned.");
            if (_mainMenuButton == null) Debug.LogWarning("PauseMenuView: Main Menu Button not assigned.");
            if (_canvasGroup == null) Debug.LogWarning("PauseMenuView: CanvasGroup not assigned.");
            if (_menuPanel == null) Debug.LogWarning("PauseMenuView: Menu Panel not assigned.");
        }
#endif
    }
}