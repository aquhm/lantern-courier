using System;
using System.IO;
using UnityEngine;

namespace LanternCourier.Save
{
    public static class SaveSerializer
    {
        const string FileName = "courier-save.json";

        public static string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public static bool Exists()
        {
            return File.Exists(FilePath);
        }

        public static void Write(SaveData data)
        {
            File.WriteAllText(FilePath, JsonUtility.ToJson(data, true));
        }

        public static bool TryRead(out SaveData data)
        {
            data = null;
            if (!File.Exists(FilePath))
            {
                return false;
            }

            try
            {
                data = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
            }
            catch (ArgumentException e)
            {
                Debug.LogWarning($"Ignoring unreadable save file: {e.Message}");
                return false;
            }

            return data != null;
        }

        public static void Delete()
        {
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
        }
    }
}
