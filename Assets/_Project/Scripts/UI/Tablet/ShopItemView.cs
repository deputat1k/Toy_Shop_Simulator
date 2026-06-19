using TMPro;
using ToyShop.Core.Interfaces;
using ToyShop.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ToyShop.UI.Tablet
{
    public class ShopItemView : MonoBehaviour
    {
        [Header("Display")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;

        [Header("Cart")]
        [SerializeField] private Button _addToCartButton;
        [SerializeField] private GameObject _cartBadgeRoot;
        [SerializeField] private TextMeshProUGUI _cartBadgeText;

        private ToyData _toyData;
        private ICartService _cart;

        public void Setup(ToyData toyData, ICartService cart)
        {
            _toyData = toyData;
            _cart = cart;

            _nameText.text = toyData.DisplayName;
            _priceText.text = $"${toyData.PurchasePrice}";

            _iconImage.sprite = toyData.Icon;
            _iconImage.enabled = toyData.Icon != null;

            _addToCartButton.onClick.RemoveAllListeners();
            _addToCartButton.onClick.AddListener(HandleAddToCart);

            _cart.OnCartChanged += UpdateCartBadge;
            UpdateCartBadge();
        }

        private void HandleAddToCart()
        {
            if (_toyData == null || _cart == null) return;
            _cart.AddItem(_toyData);
        }

        private void UpdateCartBadge()
        {
            if (_cart == null || _toyData == null) return;

            int qty = _cart.HasItem(_toyData.Id) ? _cart.GetItem(_toyData.Id).Quantity : 0;
            bool inCart = qty > 0;

            _cartBadgeRoot?.SetActive(inCart);
            if (inCart && _cartBadgeText != null)
                _cartBadgeText.text = qty.ToString();
        }

        private void OnDestroy()
        {
            if (_cart != null) _cart.OnCartChanged -= UpdateCartBadge;
            if (_addToCartButton != null) _addToCartButton.onClick.RemoveAllListeners();
        }
    }
}