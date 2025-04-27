using System;
using System.Collections.Generic;

[Serializable]
public class ChangelogEntry
{
    public string version;
    public string title;
    public string[] authors;
    public string content;
    public string date;
}

[Serializable]
public class ChangelogData
{
    public List<ChangelogEntry> entries = new List<ChangelogEntry>();
}
