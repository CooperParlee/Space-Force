using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MarkerSystem : EditorWindow {
    private const float version = 0.1f;

    List<MarkerItem> guiItems = new List<MarkerItem>();

    [MenuItem("Window/MarkerSystem")]
    public static void ShowWindow()
    {
        GetWindow<MarkerSystem>("MarkerSystem " + version);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("+"))
        {
            guiItems.Add(new MarkerItem("Marker1", new Color()));
        }
        EditorGUILayout.Foldout(true, "Active Markers:");
        EditorGUILayout.BeginScrollView(new Vector2());
        foreach(MarkerItem item in guiItems)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(item.GetName(), GUILayout.Width(100f), GUILayout.ExpandWidth(false));
            if (GUILayout.Button("-"))
            {
                guiItems.Remove(item);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }
}

public class MarkerItem
{
    string name;
    Color color;

    public MarkerItem(string name, Color color)
    {
        this.name = name;
        this.color = color;
    }

    public string GetName() { return name; }
    public Color GetColor() { return color; }
}