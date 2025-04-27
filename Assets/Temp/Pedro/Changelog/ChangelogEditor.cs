#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ChangelogManager))]
public class ChangelogEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChangelogManager manager = (ChangelogManager)target;
        if (GUILayout.Button("Adicionar Entrada ao Changelog"))
        {
            manager.AddEntry();
        }
    }
}
#endif
