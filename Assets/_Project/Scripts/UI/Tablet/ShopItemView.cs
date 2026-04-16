using System;
using TMPro;
using ToyShop.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ToyShop.UI.Tablet
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _buyButton;

        private string _toyId;
        private Action<string> _onBuyClicked;

        public class Factory : PlaceholderFactory<Transform, ShopItemView> { }

        [Inject]
        public void Construct(Transform parentContainer)
        {
            transform.SetParent(parentContainer, false);
        }

        public void Setup(ToyData toyData, Action<string> onBuyClicked)
        {
            _toyId = toyData.Id;
            _onBuyClicked = onBuyClicked;

            _nameText.text = toyData.DisplayName;
            _priceText.text = $"${toyData.PurchasePrice}";

            if (toyData.Icon != null)
            {
                _iconImage.sprite = toyData.Icon;
                _iconImage.enabled = true;
            }
            else _iconImage.enabled = false;

            _buyButton.onClick.RemoveAllListeners();
            _buyButton.onClick.AddListener(OnBuyButtonClicked);
        }

        private void OnBuyButtonClicked() => _onBuyClicked?.Invoke(_toyId);

        private void OnDestroy()
        {
            if (_buyButton != null) _buyButton.onClick.RemoveAllListeners();
        }
    }
}