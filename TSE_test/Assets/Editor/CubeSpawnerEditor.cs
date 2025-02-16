using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    private CubeSpawner _cubeSpawner;
    private int _cubesToSpawn = 1;

    public override void OnInspectorGUI()
    {
        _cubeSpawner = (CubeSpawner)target;
        _cubesToSpawn = EditorGUILayout.IntSlider("Cubes to Spawn", _cubesToSpawn, 1, 100);

        if (GUILayout.Button("Spawn Cubes"))
        {
            _cubeSpawner.SpawnCube(_cubesToSpawn);
        }

        if (GUILayout.Button("Remove All Cubes"))
        {
            _cubeSpawner.RemoveAllCubes();
        }

        DrawDefaultInspector();
    }
}
