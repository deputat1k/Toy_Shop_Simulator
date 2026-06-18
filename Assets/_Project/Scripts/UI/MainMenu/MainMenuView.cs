using System;
using System.Collections;
using ToyShop.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace ToyShop.UI.MainMenu
{
    public class MainMenuView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _loadGameButton;
        [SerializeField] private Button _exitButton;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private RectTransform _menuPanel;

        public event Action OnNewGameClicked;
        public event Action OnLoadGameClicked;
        public event Action OnExitClicked;

        private void Awake()
        {
            _newGameButton.onClick.AddListener(() => OnNewGameClicked?.Invoke());
            _loadGameButton.onClick.AddListener(() => OnLoadGameClicked?.Invoke());
            _exitButton.onClick.AddListener(() => OnExitClicked?.Invoke());

            if (_canvasGroup != null) _canvasGroup.alpha = 0f;
        }

        private void Start()
        {
            // Explicit cursor unlock — MainMenu always needs visible/unlocked cursor
            // Acts as safety net regardless of which scene transitioned here
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            StartCoroutine(PlayOpenAnimation());
        }

        private IEnumerator PlayOpenAnimation()
        {
            StartCoroutine(UIAnimator.FadeIn(_canvasGroup, 0.4f));
            yield return StartCoroutine(UIAnimator.ScaleIn(_menuPanel, 0.35f));
        }

        public void SetLoadButtonInteractable(bool interactable)
        {
            if (_loadGameButton != null)
                _loadGameButton.interactable = interactable;
        }

        private void OnDestroy()
        {
            _newGameButton.onClick.RemoveAllListeners();
            _loadGameButton.onClick.RemoveAllListeners();
            _exitButton.onClick.RemoveAllListeners();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_newGameButton == null) Debug.LogWarning("MainMenuView: New Game Button not assigned.");
            if (_loadGameButton == null) Debug.LogWarning("MainMenuView: Load Game Button not assigned.");
            if (_exitButton == null) Debug.LogWarning("MainMenuView: Exit Button not assigned.");
            if (_canvasGroup == null) Debug.LogWarning("MainMenuView: CanvasGroup not assigned.");
            if (_menuPanel == null) Debug.LogWarning("MainMenuView: Menu Panel not assigned.");
        }
#endif
    }
}