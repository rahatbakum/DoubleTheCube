using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CubeSpawner : MonoBehaviour
{
    private const float DefaultImpulse = 10f;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _cubeContainer;
    public static CubeSpawner Instance;
    private List<Cube> _spawnedCubes = new List<Cube>();

    public Cube SpawnCube(int level, Vector3 position, Quaternion rotation, bool isTakeImpulse = false)
    {
        if(!isTakeImpulse)
            return SpawnOneCube(level, position, rotation);
            
        return SpawnCubeWithImpulse(level, position, rotation, DefaultImpulse * Vector3.up);
    }

    public void DestroyCube(Cube cube)
    {
        _spawnedCubes.Remove(cube);
        Destroy(cube.gameObject);
    }

    private Cube NearestCubeByLevel(int level, Vector3 position) // returns null when there is no cube with same level
    {
        List<Cube> cubesWithThisLevel = new List<Cube>();
        foreach(var item in _spawnedCubes)
        {
            if(item.Level == level)
                cubesWithThisLevel.Add(item);
        }

        if(cubesWithThisLevel.Count == 0)
            return null;

        float nearestDistance = float.MaxValue;
        Cube nearestCube = cubesWithThisLevel[0];
        foreach(var item in cubesWithThisLevel)
        {
            float distance = Vector3.Distance(position, item.transform.position);
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCube = item;  
            }
        }
        return nearestCube;
    }

    private Cube SpawnCubeWithImpulse(int level, Vector3 position, Quaternion rotation, Vector3 impulse)
    {
        Cube cube = SpawnOneCube(level, position, rotation);
        cube.GetComponent<Rigidbody>().AddForce(impulse, ForceMode.VelocityChange);
        _spawnedCubes.Add(cube);
        return cube;
    }

    private Cube SpawnOneCube(int level, Vector3 position, Quaternion rotation)
    {
        Cube cube = (Instantiate(_cubePrefab, position, rotation, _cubeContainer) as GameObject).GetComponent<Cube>();
        cube.Initialize(level);
        return cube;
    }


    private void Awake()
    {
        Instance = this;
    }
}
