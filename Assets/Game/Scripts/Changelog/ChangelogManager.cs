using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class ChangelogManager : MonoBehaviour
{
    //Campos a serem preenchidos para o ChangeLog
    public TextMeshProUGUI testeTexto;
    public string title;
    public string[] authors;
    [TextArea(5, 10)]
    public string content;

    public string filePath;
    public ChangelogData changelogData;

    private void Awake()
    {
        filePath = Path.Combine(Application.streamingAssetsPath, "changelog.json");
        LoadChangelog();
    }

    private void LoadChangelog()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            changelogData = JsonUtility.FromJson<ChangelogData>(json);
        }
        else
        {
            changelogData = new ChangelogData();
            SaveChangelog();
        }
    }

    private void SaveChangelog()
    {
        string json = JsonUtility.ToJson(changelogData, true);
        File.WriteAllText(filePath, json);
    }

    public void AddEntry()
    {
        string newVersion = GenerateNextVersion();
        ChangelogEntry newEntry = new ChangelogEntry
        {
            version = newVersion,
            title = title,
            authors = authors,
            content = content,
            date = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        };

        changelogData.entries.Add(newEntry);
        SaveChangelog();
        Debug.Log($"Nova entrada adicionada: v{newVersion}");
    }

    private string GenerateNextVersion()
    {
        if (changelogData.entries.Count == 0)
            return "1.0.0";

        string lastVersion = changelogData.entries[changelogData.entries.Count - 1].version;
        string[] parts = lastVersion.Split('.');
        int major = int.Parse(parts[0]);
        int minor = int.Parse(parts[1]);
        int patch = int.Parse(parts[2]);

        patch++;
        return $"{major}.{minor}.{patch}";
    }

    public ChangelogEntry[] GetAllEntries()
    {
        return changelogData.entries.ToArray();
    }
}
