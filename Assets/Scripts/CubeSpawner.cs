using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CubeSpawner : MonoBehaviour
{
    private const float DefaultVelocityValueFrom0To1 = 0.3f;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private Transform _cubeContainer;
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
                cube = SpawnCubeWithVelocity(level, position, rotation, FieldHelper.DefaultVelocityDirection(), DefaultVelocityValueFrom0To1);
            else 
            {
                Debug.Log($"Nearest to {Cube.LevelToNumber(level)} is on {nearestCube.transform.position}");
                Vector3 velocityDirection = FieldHelper.VelocityDirectionByValue(PhysicsMovement.RealVelocityValue(DefaultVelocityValueFrom0To1), position, nearestCube.transform.position);   
                cube = SpawnCubeWithVelocity(level, position, rotation, velocityDirection, DefaultVelocityValueFrom0To1);
            }
        }
        AddCubeToList(cube);
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

    private Cube SpawnCubeWithoutVelocity(int level, Vector3 position, Quaternion rotation) => SpawnOneCube(level, position, rotation);

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
