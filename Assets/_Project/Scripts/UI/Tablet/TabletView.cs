using TMPro;
using UnityEngine;

namespace ToyShop.UI.Tablet
{
    [RequireComponent(typeof(CanvasGroup))]
    public class TabletView : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private TextMeshProUGUI _notificationText;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            HideNotification();
            Hide();
        }

        public Transform ItemsContainer => _itemsContainer;

        public void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            HideNotification();
        }

        public void ShowNotification(string message, Color color)
        {
            if (_notificationText == null) return;
            _notificationText.text = message;
            _notificationText.color = color;
            _notificationText.gameObject.SetActive(true);
        }

        private void HideNotification()
        {
            if (_notificationText != null) _notificationText.gameObject.SetActive(false);
        }
    }
}