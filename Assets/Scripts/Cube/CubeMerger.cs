using UnityEngine;

[DisallowMultipleComponent]
public class CubeMerger : MonoBehaviour
{
    private const float MinMergeTimeInterval = 0.1f;
    private const float CheckCollisionTimeInterval = 0.1f;

    [SerializeField] private Cube _cube;
    private CubeSpawner _cubeSpawner;
    private bool isMerged = false;
    private bool isActive = false;
    private float _lastMergeTime;
    private float _lastCheckCollisionTime;


    public void Initialize(CubeSpawner cubeSpawner)
    {
        _cubeSpawner = cubeSpawner;
    }

    public void Activate()
    {
        isActive = true;
        SaveLastMergeTime(Time.time - MinMergeTimeInterval); //some random small value 
        SaveLastCheckCollisionTime(Time.time - CheckCollisionTimeInterval); //some random small value 
    }

    public bool TryMergeCube(CubeMerger cubeMerger)
    {   
        if(!CheckIsCanMerge(cubeMerger))
            return false;

        isMerged = true;
        cubeMerger.isMerged = true;

        Cube newCube = SpawnNewCube(cubeMerger);
        
        _cube.OnMerging(newCube);
        cubeMerger._cube.OnMerging(newCube);
        
        _cubeSpawner.DestroyCube(_cube);
        _cubeSpawner.DestroyCube(cubeMerger._cube);

        newCube.GetComponent<CubeMerger>().SaveLastMergeTime(Time.time);
        return true;
    }

    private Cube SpawnNewCube(CubeMerger cubeMerger)
    {
        int newCubeLevel = NewCubeLevel(_cube.Level, cubeMerger._cube.Level);
        Vector3 newCubePosition = NewCubePosition(_cube.transform.position, cubeMerger._cube.transform.position);
        Quaternion newCubeRotation = NewCubeRotation(_cube.transform.rotation, cubeMerger._cube.transform.rotation);
        return _cubeSpawner.SpawnCubeAfterMerge(newCubeLevel, newCubePosition, newCubeRotation, isTakeVelocity: true);
    }

    private bool CheckIsCanMerge(CubeMerger cubeMerger)
    {
        if(Time.time < _lastMergeTime + MinMergeTimeInterval)
            return false;
        if(Time.time < cubeMerger._lastMergeTime + MinMergeTimeInterval)
            return false;
        if(isMerged)
            return false;
        if(cubeMerger.isMerged)
            return false;
        if(!isActive)
            return false;
        if(!cubeMerger.isActive)
            return false;
        if(_cube == null || cubeMerger._cube == null)
            return false;
        if(!IsSuitableCubes(_cube,cubeMerger._cube))
            return false;
        return true;
    }

    private static Vector3 NewCubePosition(Vector3 position1, Vector3 position2) => MathHelper.Half(position1 + position2);
    
    private static Quaternion NewCubeRotation(Quaternion rotation1, Quaternion  rotation2) => rotation1;

    private static int NewCubeLevel(int cube1Level, int cube2Level) => cube1Level + 1;

    private static Cube SlowerCube(Cube cube1, Cube cube2)
    {
        Rigidbody cube1RigidBody;
        Rigidbody cube2RigidBody;
        if(!cube1.TryGetComponent<Rigidbody>(out cube1RigidBody) || !cube2.TryGetComponent<Rigidbody>(out cube2RigidBody))
            return cube1;
        if(cube1RigidBody.velocity.magnitude <= cube2RigidBody.velocity.magnitude)
            return cube1;
        return cube2;
    }

    private static bool IsSuitableCubes(Cube cube1, Cube cube2) => cube1.Level == cube2.Level;

    private void OnCollisionStay(Collision other)
    {
        if(Time.time > _lastCheckCollisionTime + CheckCollisionTimeInterval)
            CheckCollision(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        CheckCollision(other);
    }

    private void CheckCollision(Collision other)
    {
        SaveLastCheckCollisionTime(Time.time);
        if(other.gameObject.TryGetComponent(out CubeMerger cubeMerger))
            TryMergeCube(cubeMerger);
    }
    
    private void SaveLastMergeTime(float time) => _lastMergeTime = time;
    private void SaveLastCheckCollisionTime(float time) => _lastCheckCollisionTime = time;

    private void OnDrawGizmos()
    {
        if(Time.time < _lastMergeTime + MinMergeTimeInterval)
            Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
