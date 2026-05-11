using UnityEngine;

namespace ToyShop.Gameplay.NPC
{
    public class NpcItemVisual : MonoBehaviour
    {
        [Header("Hand Attachment")]
        [Tooltip("Assign the basket/item GameObject attached to hand bone")]
        [SerializeField] private GameObject _itemVisualObject;

        private void Awake()
        {
            // If not assigned in Inspector — create fallback sphere
            if (_itemVisualObject == null)
                _itemVisualObject = CreateFallbackVisual();

            _itemVisualObject.SetActive(false);
        }

        public void Show()
        {
            if (_itemVisualObject != null)
                _itemVisualObject.SetActive(true);
        }

        public void Hide()
        {
            if (_itemVisualObject != null)
                _itemVisualObject.SetActive(false);
        }

        // Fallback sphere — used until real model is assigned
        private GameObject CreateFallbackVisual()
        {
            GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            visual.transform.SetParent(transform);
            visual.transform.localPosition = new Vector3(0f, 1.8f, 0.3f);
            visual.transform.localScale = Vector3.one * 0.3f;
            Destroy(visual.GetComponent<Collider>());
            return visual;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_itemVisualObject == null)
                Debug.LogWarning("NpcItemVisual: Item Visual Object not assigned. Will use fallback sphere.");
        }
#endif
    }
}