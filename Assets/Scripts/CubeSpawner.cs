using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CubeSpawner : MonoBehaviour
{
    private const float DefaultImpulseValueFrom0To1 = 0.6f;

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
        {
            Cube nearestCube = FieldHelper.NearestCubeByLevel(_spawnedCubes, level, position);
            if(nearestCube == null)
                cube = SpawnCubeWithImpulse(level, position, rotation, FieldHelper.DefaultImpulseDirection(), DefaultImpulseValueFrom0To1);
            else 
            {
                Debug.Log($"Nearest to {level} is on {Vector3.Distance(position, nearestCube.transform.position)}");
                Vector3 impulseDirection = FieldHelper.ImpulseDirectionByValue(PhysicsMovement.RealImpulseValue(DefaultImpulseValueFrom0To1), position, nearestCube.transform.position);   
                cube = SpawnCubeWithImpulse(level, position, rotation, impulseDirection, DefaultImpulseValueFrom0To1);
            }
        }
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

    private Cube SpawnCubeWithImpulse(int level, Vector3 position, Quaternion rotation, Vector3 impulseDirection, float impulseValueFrom0To1)
    {
        Cube cube = SpawnOneCube(level, position, rotation);
        cube.GetComponent<PhysicsMovement>().AddImpulse(impulseDirection, impulseValueFrom0To1, isAddTorque: true);
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
