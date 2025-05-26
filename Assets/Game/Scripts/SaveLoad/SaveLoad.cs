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
            //ResetSave();
            Load();
        }
        else
        {
            Destroy(this);
        }
    }
    public void ResetSave()
    {
        saveData = new SaveData();
        saveData.player = new PlayerData("Roberto");
        string json = JsonUtility.ToJson(saveData);
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
        Debug.Log(saveData);
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
    public string name = "Roff Temp";
    public int strenght = 0, agility = 0, constitution = 0;
    public int primaryRune = 0, secondaryRune = 0, TerciaryRune = 0;
    public float brightness = 0;
    public int davidDialogueIndex = 0, annelieseDialogueIndex = 0;
    public float skillPoints = 0;
    public int[] runeValue = new int[0];
    public bool[] runePurchased = new bool[0];
    public PlayerData(string name)
    {
        this.name = name;
    }
}
#endregion