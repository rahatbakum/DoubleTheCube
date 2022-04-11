using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CubeSpawner : MonoBehaviour
{
    private const float MinVelocityValueFrom0To1 = 0.2f;
    private const float MaxVelocityValueFrom0To1 = 0.4f;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _cubeContainer;
    [SerializeField] private UnityEvent<Cube> _cubeSpawnedAfterMerge;
    public event UnityAction<Cube> CubeSpawnedAfterMerge
    {
        add => _cubeSpawnedAfterMerge.AddListener(value);
        remove => _cubeSpawnedAfterMerge.RemoveListener(value);
    }
    [SerializeField] private UnityEvent<Cube> _cubeSpawned;
    public event UnityAction<Cube> CubeSpawned
    {
        add => _cubeSpawned.AddListener(value);
        remove => _cubeSpawned.RemoveListener(value);
    }
    public static CubeSpawner Instance;
    private List<Cube> _spawnedCubes = new List<Cube>();

    public Cube SpawnCubeInField(int level, Vector3 position, Quaternion rotation, bool isTakeVelocity = false)
    {
        Cube cube;
        if(!isTakeVelocity)
            cube = SpawnCubeWithoutVelocity(level, position, rotation);
        else
        {
            Cube nearestCube = FieldHelper.NearestCubeByLevel(_spawnedCubes, level, position);
            if(nearestCube == null)
                cube = SpawnCubeWithVelocity(level, position, rotation, FieldHelper.DefaultVelocityDirection(), VelocityValueFrom0To1());
            else 
            {
                Vector3 velocityDirection = FieldHelper.VelocityDirectionByValue(PhysicsMovement.RealVelocityValue(VelocityValueFrom0To1()), position, nearestCube.transform.position);   
                cube = SpawnCubeWithVelocity(level, position, rotation, velocityDirection, VelocityValueFrom0To1());
            }
        }
        AddCubeToList(cube);
        return cube;
    }

    public Cube SpawnCubeAfterMerge(int level, Vector3 position, Quaternion rotation, bool isTakeVelocity = false)
    {
        Cube cube = SpawnCubeInField(level, position, rotation, isTakeVelocity);
        _cubeSpawnedAfterMerge?.Invoke(cube);
        cube.OnSpawnedAfterMerge();
        return cube;
    }

    public Cube SpawnCubeOutsideField(int level, Vector3 position, Quaternion rotation) => SpawnCubeWithoutVelocity(level, position, rotation);

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

    private Cube SpawnCubeWithVelocity(int level, Vector3 position, Quaternion rotation, Vector3 velocityDirection, float velocityValueFrom0To1)
    {
        Cube cube = SpawnOneCube(level, position, rotation);
        cube.GetComponent<PhysicsMovement>().AddVelocity(velocityDirection, velocityValueFrom0To1, isAddTorque: true);
        return cube;
    }

    private static float VelocityValueFrom0To1() => Random.Range(MinVelocityValueFrom0To1, MaxVelocityValueFrom0To1);

    private Cube SpawnCubeWithoutVelocity(int level, Vector3 position, Quaternion rotation) => SpawnOneCube(level, position, rotation);

    private Cube SpawnOneCube(int level, Vector3 position, Quaternion rotation)
    {
        Cube cube = (Instantiate(_cubePrefab, position, rotation) as GameObject).GetComponent<Cube>();
        cube.Initialize(level);
        _cubeSpawned?.Invoke(cube);
        return cube;
    }

    private void Awake()
    {
        if(Instance != null)
            throw new System.Exception("Here is two CubeSpawner in the scene");
        else
            Instance = this;
    }
}
