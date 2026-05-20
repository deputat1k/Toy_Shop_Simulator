using System.Collections.Generic;
using ToyShop.Core.Interfaces;
using ToyShop.Core.SaveSystem;
using ToyShop.Gameplay.Items;
using UnityEngine;

namespace ToyShop.Gameplay.SaveSystem
{
    public class BoxSaveHandler : ISaveHandler
    {
        private readonly ICatalogService _catalog;
        private readonly BoxContainer.Factory _boxFactory;
        private readonly IPlayerController _player;

        public BoxSaveHandler(
            ICatalogService catalog,
            BoxContainer.Factory boxFactory,
            IPlayerController player)
        {
            _catalog = catalog;
            _boxFactory = boxFactory;
            _player = player;
        }

        public void OnSave(GameSaveData saveData)
        {
            saveData.Boxes = new List<BoxSaveData>();

            BoxContainer[] allBoxes =
                Object.FindObjectsByType<BoxContainer>(FindObjectsSortMode.None);

            foreach (BoxContainer box in allBoxes)
            {
                IItemGrabbable grabbable = box.GetComponent<IItemGrabbable>();
                bool isHeld = grabbable != null && grabbable.IsHeld;

                saveData.Boxes.Add(new BoxSaveData
                {
                    ToyId = box.ToyData != null ? box.ToyData.Id : string.Empty,
                    ItemCount = box.ItemCount,
                    // Held box: save at player's feet — player stands on the ground
                    Position = isHeld ? _player.Transform.position : box.transform.position,
                    Rotation = box.transform.rotation
                });
            }
        }

        public void OnLoad(GameSaveData saveData)
        {
            DestroyAllBoxes();

            if (saveData.Boxes == null || saveData.Boxes.Count == 0) return;

            foreach (BoxSaveData boxData in saveData.Boxes)
                RestoreBox(boxData);
        }

        private void DestroyAllBoxes()
        {
            BoxContainer[] allBoxes =
                Object.FindObjectsByType<BoxContainer>(FindObjectsSortMode.None);

            foreach (BoxContainer box in allBoxes)
            {
                IItemGrabbable grabbable = box.GetComponent<IItemGrabbable>();
                if (grabbable != null && grabbable.IsHeld)
                    grabbable.Drop();

                Object.Destroy(box.gameObject);
            }
        }

        private void RestoreBox(BoxSaveData boxData)
        {
            BoxContainer box = _boxFactory.Create();
            box.transform.position = boxData.Position;
            box.transform.rotation = boxData.Rotation;

            if (string.IsNullOrEmpty(boxData.ToyId)) return;

            var toyData = _catalog.GetToyById(boxData.ToyId);
            if (toyData == null)
            {
                Debug.LogWarning($"[BoxSaveHandler] ToyData not found for id: {boxData.ToyId}");
                return;
            }

            box.SetupBox(toyData, boxData.ItemCount);
        }
    }
}