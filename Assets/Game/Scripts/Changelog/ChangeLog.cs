using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ChangeLog : MonoBehaviour
{
    public ChangelogManager changelog;
    public ChangelogData changelogData;
    string fullChangeLog;
    public GameObject textScroller;
    public GameObject logger;
    public TextMeshProUGUI version;
    void Start()
    {
        changelogData = changelog.changelogData;
        foreach( var e in changelogData.entries)
        {
            TextMeshProUGUI teste = Instantiate(logger, textScroller.transform).GetComponent<TextMeshProUGUI>();
            teste.transform.SetParent(textScroller.transform, false);
            string authors = "";
            foreach (string author in e.authors)
            {
                authors = authors + ", " + author;
            }
            teste.text = $"<color=#F9FF00>{e.title}</color>: " + e.content + $"<color=#00C6B5>{authors}</color>: ";
        }
        if(changelogData.entries.Count > 0) version.text = "V " + changelogData.entries[changelogData.entries.Count-1].version + " ChangeLog(click)";
    }
}
