using System;
using System.Collections;
using TMPro;
using ToyShop.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace ToyShop.UI.Tablet
{
    public class TabletView : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private ShopItemView _shopItemPrefab;
        [SerializeField] private CartItemView _cartItemPrefab;

        [Header("Pages")]
        [SerializeField] private GameObject _shopPage;
        [SerializeField] private GameObject _cartPage;
        [SerializeField] private Transform _shopItemsContainer;
        [SerializeField] private Transform _cartItemsContainer;

        [Header("Header")]
        [SerializeField] private TextMeshProUGUI _balanceText;
        [SerializeField] private Button _closeButton;

        [Header("Tabs")]
        [SerializeField] private Button _shopTabButton;
        [SerializeField] private Button _cartTabButton;
        [SerializeField] private Image _shopTabHighlight;
        [SerializeField] private Image _cartTabHighlight;
        [SerializeField] private GameObject _cartBadgeRoot;
        [SerializeField] private TextMeshProUGUI _cartBadgeText;

        [Header("Cart Footer")]
        [SerializeField] private TextMeshProUGUI _totalPriceText;
        [SerializeField] private Button _buyAllButton;
        [SerializeField] private GameObject _emptyCartMessage;

        [Header("Notification")]
        [SerializeField] private TextMeshProUGUI _notificationText;

        [Header("Animation")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _openDuration = 0.25f;
        [SerializeField] private float _closeDuration = 0.18f;

        [Header("Tab Colors")]
        [SerializeField] private Color _tabActiveColor = new Color(0.18f, 0.49f, 0.80f);
        [SerializeField] private Color _tabInactiveColor = new Color(0.10f, 0.20f, 0.37f);

        public event Action OnCloseClicked;
        public event Action OnBuyAllClicked;
        public event Action OnShopTabClicked;
        public event Action OnCartTabClicked;

        // Exposed for TabletPresenter — Instantiate uses these
        public ShopItemView ShopItemPrefab => _shopItemPrefab;
        public CartItemView CartItemPrefab => _cartItemPrefab;
        public Transform ShopItemsContainer => _shopItemsContainer;
        public Transform CartItemsContainer => _cartItemsContainer;

        private Coroutine _hideRoutine;
        private Coroutine _notificationRoutine;

        private void Awake()
        {
            _closeButton?.onClick.AddListener(() => OnCloseClicked?.Invoke());
            _buyAllButton?.onClick.AddListener(() => OnBuyAllClicked?.Invoke());
            _shopTabButton?.onClick.AddListener(() => OnShopTabClicked?.Invoke());
            _cartTabButton?.onClick.AddListener(() => OnCartTabClicked?.Invoke());

            HideNotification();
            if (_canvasGroup != null) _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
        }

        // ── Show / Hide — fade only, no slide ─────────────────────────

        public void Show()
        {
            if (_hideRoutine != null) { StopCoroutine(_hideRoutine); _hideRoutine = null; }
            gameObject.SetActive(true);
            StartCoroutine(UIAnimator.FadeIn(_canvasGroup, _openDuration));
        }

        public void Hide()
        {
            _hideRoutine = StartCoroutine(PlayHideAnimation());
        }

        private IEnumerator PlayHideAnimation()
        {
            yield return StartCoroutine(UIAnimator.FadeOut(_canvasGroup, _closeDuration));
            HideNotification();
            gameObject.SetActive(false);
            _hideRoutine = null;
        }

        // ── Tabs ───────────────────────────────────────────────────────

        public void ShowShopPage()
        {
            _shopPage?.SetActive(true);
            _cartPage?.SetActive(false);
            SetTabHighlight(shopActive: true);
        }

        public void ShowCartPage()
        {
            _shopPage?.SetActive(false);
            _cartPage?.SetActive(true);
            SetTabHighlight(shopActive: false);
        }

        private void SetTabHighlight(bool shopActive)
        {
            if (_shopTabHighlight != null)
                _shopTabHighlight.color = shopActive ? _tabActiveColor : _tabInactiveColor;
            if (_cartTabHighlight != null)
                _cartTabHighlight.color = shopActive ? _tabInactiveColor : _tabActiveColor;
        }

        // ── Data ───────────────────────────────────────────────────────

        public void UpdateBalance(int amount)
        {
            if (_balanceText != null)
                _balanceText.text = $"${amount:N0}";
        }

        public void UpdateCartBadge(int totalItems)
        {
            bool hasItems = totalItems > 0;
            _cartBadgeRoot?.SetActive(hasItems);
            if (hasItems && _cartBadgeText != null)
                _cartBadgeText.text = totalItems.ToString();
        }

        public void UpdateCartTotal(int totalPrice, bool isEmpty)
        {
            if (_totalPriceText != null) _totalPriceText.text = $"${totalPrice:N0}";
            if (_buyAllButton != null) _buyAllButton.interactable = !isEmpty;
            if (_emptyCartMessage != null) _emptyCartMessage.SetActive(isEmpty);
        }

        // ── Notification ───────────────────────────────────────────────

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
            _notificationText?.gameObject.SetActive(false);
        }

        private IEnumerator HideNotificationAfterDelay()
        {
            yield return new WaitForSecondsRealtime(2f);
            HideNotification();
        }

        private void OnDestroy()
        {
            _closeButton?.onClick.RemoveAllListeners();
            _buyAllButton?.onClick.RemoveAllListeners();
            _shopTabButton?.onClick.RemoveAllListeners();
            _cartTabButton?.onClick.RemoveAllListeners();
        }
    }
}