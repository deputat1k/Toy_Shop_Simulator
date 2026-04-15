
using UnityEngine;
using TMPro;
namespace ToyShop.UI.HUD
{
    [RequireComponent(typeof(CanvasGroup))]
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _balanceText;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetBalance(int currentBalance)
        {
            if (_balanceText != null)
            {
                _balanceText.text = $"${currentBalance}";
            }
        }

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
        }
    }
}