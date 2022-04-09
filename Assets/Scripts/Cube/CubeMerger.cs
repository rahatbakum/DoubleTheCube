using UnityEngine;

[DisallowMultipleComponent]
public class CubeMerger : MonoBehaviour
{
    private const float MinMergeTimeInterval = 0.25f;
    private const float CheckCollisionTimeInterval = 0.25f;

    [SerializeField] private Cube _cube;
    private bool isMerged = false;
    private bool isActive = false;
    private float _lastMergeTime;
    private float _lastCheckCollisionTime;

    public void Activate()
    {
        isActive = true;
        SaveLastMergeTime(Time.time - MinMergeTimeInterval); //some random small value 
        SaveLastCheckCollisionTime(Time.time - CheckCollisionTimeInterval); //some random small value 
    }

    public bool TryMergeCube(CubeMerger cubeMerger)
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
        if(CubeSpawner.Instance == null)
            return false;

        isMerged = true;
        cubeMerger.isMerged = true;

        //Debug.Log($"Merge {Cube.LevelToNumber(_cube.Level)} {Cube.LevelToNumber(cubeMerger._cube.Level)}");
        Cube slowerCube = SlowerCube(_cube, cubeMerger._cube);
        int newCubeLevel = NewCubeLevel(_cube.Level, cubeMerger._cube.Level);
        Vector3 newCubePosition = slowerCube.transform.position;
        Quaternion newCubeRotation = slowerCube.transform.rotation;

        CubeSpawner.Instance.DestroyCube(_cube);
        CubeSpawner.Instance.DestroyCube(cubeMerger._cube);

        Cube newCube = CubeSpawner.Instance.SpawnCubeInField(newCubeLevel, newCubePosition, newCubeRotation, isTakeImpulse: true);
        newCube.GetComponent<CubeMerger>().SaveLastMergeTime(Time.time);
        return true;
    }

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
        if(Time.time > _lastCheckCollisionTime + CheckCollisionTimeInterval)
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
