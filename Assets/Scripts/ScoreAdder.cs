using UnityEngine;

public class ScoreAdder : MonoBehaviour
{
    [SerializeField] private CubeSpawner _cubeSpawner;
    
    private void OnCubeSpawnedAfterMerge(Cube cube)
    {
        GameManager.Instance.AddScore(cube.Number);
    }

    private void OnEnable()
    {
        _cubeSpawner.CubeSpawnedAfterMerge += OnCubeSpawnedAfterMerge;
    }

    private void OnDisable()
    {
        _cubeSpawner.CubeSpawnedAfterMerge -= OnCubeSpawnedAfterMerge;
    }
}
