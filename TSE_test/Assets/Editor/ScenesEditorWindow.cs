using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;

public class ScenesEditorWindow : EditorWindow
{
    private List<string> _sceneNames = new List<string>();
    private List<string> _scenePaths = new List<string>();

    [MenuItem("Window/Scene List Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<ScenesEditorWindow>("Scene List");
        window.Show();
    }

    private void OnEnable()
    {
        LoadScenes();
    }

    private void OnGUI()
    {
        GUILayout.Label("Scenes in Build Settings", EditorStyles.boldLabel);

        for (int i = 0; i < _sceneNames.Count; i++)
        {
            if (GUILayout.Button(_sceneNames[i]))
            {
                LoadScene(i);
            }
        }

        var dropArea = GUILayoutUtility.GetRect(0, 100, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag and Drop Scene Here to Add");

        HandleDragAndDrop(dropArea);
    }

    private void LoadScenes()
    {
        _sceneNames.Clear();
        _scenePaths.Clear();

        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                _sceneNames.Add(scene.path);
                _scenePaths.Add(scene.path);
            }
        }
    }

    private void LoadScene(int index)
    {
        if (index >= 0 && index < _sceneNames.Count)
        {
            var scenePath = _scenePaths[index];
            EditorSceneManager.OpenScene(scenePath);
            Debug.Log("Cargando escena: " + scenePath);
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        var e = Event.current;

        if (dropArea.Contains(e.mousePosition))
        {
            switch (e.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    DragPerform();
                    break;
            }
        }

        void DragPerform()
        {
            if (DragAndDrop.objectReferences.Length > 0)
            {
                foreach (var obj in DragAndDrop.objectReferences)
                {
                    if (obj is SceneAsset)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Link;
                        if (e.type == EventType.DragPerform)
                        {
                            var scenePath = AssetDatabase.GetAssetPath(obj);

                            AddSceneToBuildSettings(scenePath);
                            LoadScenes();
                            Repaint();
                        }
                    }
                    else
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    }
                }
            }
        }
    }

    // Añadir la escena a los Build Settings
    private void AddSceneToBuildSettings(string scenePath)
    {
        foreach (var scene in EditorBuildSettings.scenes)
        {
            if (scene.path == scenePath)
            {
                Debug.Log("La escena ya está en los Build Settings.");
                return;
            }
        }

        var newScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        newScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = newScenes.ToArray();

        Debug.Log("Escena añadida a los Build Settings: " + scenePath);
    }
}
