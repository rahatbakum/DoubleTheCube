using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CubeSpawner : MonoBehaviour
{
    private const float DefaultImpulseFrom0To1 = 0.6f;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _cubeContainer;
    public static CubeSpawner Instance;
    private List<Cube> _spawnedCubes = new List<Cube>();

    public Cube SpawnCubeInField(int level, Vector3 position, Quaternion rotation, bool isTakeImpulse = false)
    {
        Cube cube;
        if(!isTakeImpulse)
            cube = SpawnCubeWithoutImpulse(level, position, rotation);
        else
            cube = SpawnCubeWithImpulse(level, position, rotation, Vector3.up, DefaultImpulseFrom0To1);
        AddCubeToList(cube);
        return cube;
    }

    public Cube SpawnCubeOutsideField(int level, Vector3 position, Quaternion rotation) => SpawnCubeWithoutImpulse(level, position, rotation);

    public void DestroyCube(Cube cube)
    {
        _spawnedCubes.Remove(cube);
        Destroy(cube.gameObject);
    }

    public void AddCubeToList(Cube cube)
    {
        if(_spawnedCubes.Exists((Cube cubeInList) => cubeInList == cube))
            return;
        _spawnedCubes.Add(cube);
        cube.transform.SetParent(_cubeContainer);
        cube.Activate();
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

    private Cube SpawnCubeWithImpulse(int level, Vector3 position, Quaternion rotation, Vector3 impulseDirection, float impulseFrom0To1)
    {
        Cube cube = SpawnOneCube(level, position, rotation);
        cube.GetComponent<PhysicsMovement>().AddImpulse(impulseDirection, impulseFrom0To1);
        return cube;
    }

    private Cube SpawnCubeWithoutImpulse(int level, Vector3 position, Quaternion rotation) => SpawnOneCube(level, position, rotation);

    private Cube SpawnOneCube(int level, Vector3 position, Quaternion rotation)
    {
        Cube cube = (Instantiate(_cubePrefab, position, rotation) as GameObject).GetComponent<Cube>();
        cube.Initialize(level);
        return cube;
    }


    private void Awake()
    {
        Instance = this;
    }
}
