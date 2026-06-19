using TMPro;
using ToyShop.Core.Interfaces;
using ToyShop.Gameplay.Cart;
using UnityEngine;
using UnityEngine.UI;

namespace ToyShop.UI.Tablet
{
    public class CartItemView : MonoBehaviour
    {
        [Header("Display")]
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _unitPriceText;
        [SerializeField] private TextMeshProUGUI _lineTotalText;
        [SerializeField] private TextMeshProUGUI _quantityText;

        [Header("Controls")]
        [SerializeField] private Button _increaseButton;
        [SerializeField] private Button _decreaseButton;
        [SerializeField] private Button _removeButton;

        private CartItem _cartItem;
        private ICartService _cart;

        public void Setup(CartItem cartItem, ICartService cart)
        {
            _cartItem = cartItem;
            _cart = cart;

            _nameText.text = cartItem.ToyData.DisplayName;
            _unitPriceText.text = $"${cartItem.ToyData.PurchasePrice} ea";

            _iconImage.sprite = cartItem.ToyData.Icon;
            _iconImage.enabled = cartItem.ToyData.Icon != null;

            _increaseButton.onClick.RemoveAllListeners();
            _decreaseButton.onClick.RemoveAllListeners();
            _removeButton.onClick.RemoveAllListeners();

            _increaseButton.onClick.AddListener(() => _cart.ChangeQuantity(_cartItem.ToyData.Id, +1));
            _decreaseButton.onClick.AddListener(() => _cart.ChangeQuantity(_cartItem.ToyData.Id, -1));
            _removeButton.onClick.AddListener(() => _cart.RemoveItem(_cartItem.ToyData.Id));

            _cart.OnCartChanged += UpdateDisplay;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (!_cart.HasItem(_cartItem.ToyData.Id)) return;

            _quantityText.text = _cartItem.Quantity.ToString();
            _lineTotalText.text = $"${_cartItem.LineTotal}";
        }

        private void OnDestroy()
        {
            if (_cart != null)
                _cart.OnCartChanged -= UpdateDisplay;

            _increaseButton?.onClick.RemoveAllListeners();
            _decreaseButton?.onClick.RemoveAllListeners();
            _removeButton?.onClick.RemoveAllListeners();
        }
    }
}