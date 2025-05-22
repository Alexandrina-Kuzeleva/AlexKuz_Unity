using System.IO;
using UnityEngine;

public class SaveSystemJSON
{
    private static string path = Application.persistentDataPath + "/save.json";

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
        Debug.Log("Game saved to JSON.");
    }

    public static SaveData Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<SaveData>(json);
        }

        Debug.LogWarning("No save file found.");
        return null;
    }

    public static bool SaveExists()
    {
        return File.Exists(path);
    }
}
