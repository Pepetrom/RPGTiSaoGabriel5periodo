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
    public RectTransform contentSize;
    void Start()
    {
        changelogData = changelog.changelogData;
        foreach( var e in changelogData.entries)
        {
            GameObject tempLogger = Instantiate(logger, textScroller.transform);
            TextMeshProUGUI loggerText = tempLogger.GetComponentInChildren<TextMeshProUGUI>();
            tempLogger.transform.SetParent(textScroller.transform, false);
            string authors = "";
            foreach (string author in e.authors)
            {
                authors = authors + ", " + author;
            }
            loggerText.text = $"<color=#F9FF00>{e.version}</color> ({e.date}) <color=#F9FF00>{e.title}</color>:\n {e.content} <color=#00C6B5>{authors}</color>: ";
        }
        contentSize.sizeDelta = new Vector2(contentSize.sizeDelta.x, changelogData.entries.Count * 150) ;
        if(changelogData.entries.Count > 0) version.text = "V " + changelogData.entries[changelogData.entries.Count-1].version + " ChangeLog(click)";
    }
}
