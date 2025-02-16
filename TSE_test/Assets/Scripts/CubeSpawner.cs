using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private TextMeshProUGUI _cubeCountText;

    private List<GameObject> _cubes = new List<GameObject>();
    private float _posRange = 10f;
    private int _cubeCount = 0;

    public void SpawnCube(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            var randomPosition = new Vector3(
                GetRandom(),
                GetRandom(),
                GetRandom()
            );

            var cube = Instantiate(_cubePrefab, randomPosition, Quaternion.identity);
            
            _cubes.Add(cube);
            _cubeCount++;

            AssignRandomMaterial(cube);
            UpdateCubeCountText();
        }

        float GetRandom()
        {
            return Random.Range(-_posRange, _posRange);
        }
    }

    private void AssignRandomMaterial(GameObject cube)
    {
        var materials = Resources.LoadAll<Material>("Materials");
        if (materials.Length > 0)
        {
            cube.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
        }
    }

    private void UpdateCubeCountText()
    {
        if (_cubeCountText != null)
        {
            _cubeCountText.text = "Cubes count: " + _cubeCount;
        }
    }


    public void RemoveAllCubes()
    {
        foreach (GameObject cube in _cubes)
        {
            DestroyImmediate(cube);
        }

        _cubes.Clear();
        _cubeCount = 0;
        UpdateCubeCountText();
    }
}
