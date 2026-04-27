using TMPro;
using UnityEngine;

namespace ToyShop.UI.HUD
{
    public class CurrencyView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _balanceText;

        public void SetBalance(int balance)
        {
            if (_balanceText != null)
                _balanceText.text = $"${balance}";
        }

        public void Show() => gameObject.SetActive(true);
        public void Hide() => gameObject.SetActive(false);

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_balanceText == null)
                _balanceText = GetComponentInChildren<TextMeshProUGUI>();
        }
#endif
    }
}