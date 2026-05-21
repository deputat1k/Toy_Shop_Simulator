using System;
using System.Collections.Generic;
using System.IO;
using ToyShop.Core.Interfaces;
using ToyShop.Core.SaveSystem;
using UnityEngine;

namespace ToyShop.Gameplay.SaveSystem
{
    public class SaveService : ISaveService
    {
        private readonly List<ISaveHandler> _handlers;

        private const string SaveFileName = "save.json";
        private const int CurrentSaveVersion = 1;

        private string SavePath => Path.Combine(Application.persistentDataPath, SaveFileName);

        public bool HasSave => File.Exists(SavePath);

        public SaveService(List<ISaveHandler> handlers)
        {
            _handlers = handlers;
        }

        public void Save()
        {
            try
            {
                var data = new GameSaveData();

                foreach (ISaveHandler handler in _handlers)
                    handler.OnSave(data);

                string json = JsonUtility.ToJson(data, prettyPrint: true);
                File.WriteAllText(SavePath, json);

                Debug.Log($"[SaveService] Saved to: {SavePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveService] Save failed: {e.Message}");
            }
        }

        public void Load()
        {
            if (!HasSave)
            {
                Debug.LogWarning("[SaveService] No save file found.");
                return;
            }

            try
            {
                string json = File.ReadAllText(SavePath);
                GameSaveData data = JsonUtility.FromJson<GameSaveData>(json);

                // JsonUtility returns null for empty or structurally invalid JSON
                if (data == null)
                {
                    Debug.LogError("[SaveService] Save file is corrupted or empty. Load aborted.");
                    return;
                }

                if (data.SaveVersion != CurrentSaveVersion)
                {
                    Debug.LogWarning($"[SaveService] Save version mismatch. " +
                                     $"Expected {CurrentSaveVersion}, got {data.SaveVersion}. Load aborted.");
                    return;
                }

                foreach (ISaveHandler handler in _handlers)
                    handler.OnLoad(data);

                Debug.Log($"[SaveService] Loaded from: {SavePath}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveService] Load failed: {e.Message}");
            }
        }
    }
}