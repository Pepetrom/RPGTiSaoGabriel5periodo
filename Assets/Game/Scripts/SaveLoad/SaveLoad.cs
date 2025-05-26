using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    public static SaveLoad instance;
    public string path;
    public SaveData saveData;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
            path = Path.Combine(Application.persistentDataPath, "save.json");
            Load();
        }
        else
        {
            Destroy(this);
        }
    }
    public void ResetSave()
    {
        string json = "";
        File.WriteAllText(path, json);
        Debug.LogWarning("Progresso apagado.");
    }
    public void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);
        Debug.LogWarning("Jogo foi salvo.");
    }
    public void Load()
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log(json);
            Debug.LogWarning("Arquivo de save encontrado.");
        }
        else
        {
            Debug.LogWarning("Arquivo de save nao encontrado.");
            saveData = new SaveData();
            saveData.player = new PlayerData("Roff");
            Debug.LogWarning("Arquivo de save criado.");
            Save();
        }
        DebugSave();
    }
    void DebugSave()
    {
        Debug.Log($"Este save e de {saveData.player.name}");
        Debug.Log($"A config de luz é: {saveData.player.brightness}");
    }
}
#region OtherClassesToSave
[System.Serializable]
public class SaveData
{
    public PlayerData player;
}
[System.Serializable]
public class PlayerData
{
    public string name;
    public int primaryRune = 0, secondaryRune = 0, TerciaryRune = 0;
    public float brightness;
    public int davidDialogueIndex, annelieseDialogueIndex;
    public PlayerData(string name)
    {
        this.name = name;
    }
}
#endregion