using TMPro;
using UnityEngine;

namespace ToyShop.UI.Tablet
{
    public class TabletView : MonoBehaviour
    {
        [SerializeField] private Transform _itemsContainer;
        [SerializeField] private TextMeshProUGUI _notificationText;

        public Transform ItemsContainer => _itemsContainer;

        private void Awake()
        {
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
        }

        private void HideNotification()
        {
            if (_notificationText != null)
                _notificationText.gameObject.SetActive(false);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_itemsContainer == null)
                _itemsContainer = transform.Find("ItemsContainer");

            if (_notificationText == null)
                _notificationText = GetComponentInChildren<TextMeshProUGUI>();
        }
#endif
    }
}