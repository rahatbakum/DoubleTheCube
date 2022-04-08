using UnityEngine;

[DisallowMultipleComponent]
public class CubeMerger : MonoBehaviour
{
    private const float MinMergeTimeInterval = 0.2f;
    [SerializeField] private Cube _cube;
    private bool isMerged = false;
    private bool isActive = false;
    private float _lastMergeTime;

    public void Activate()
    {
        isActive = true;
        SaveLastMergeTime();
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
        SaveLastMergeTime();

        Debug.Log($"Merge {Cube.LevelToNumber(_cube.Level)} {Cube.LevelToNumber(cubeMerger._cube.Level)}");
        Cube slowerCube = SlowerCube(_cube, cubeMerger._cube);
        int newCubeLevel = NewCubeLevel(_cube.Level, cubeMerger._cube.Level);
        Vector3 newCubePosition = slowerCube.transform.position;
        Quaternion newCubeRotation = slowerCube.transform.rotation;

        CubeSpawner.Instance.DestroyCube(_cube);
        CubeSpawner.Instance.DestroyCube(cubeMerger._cube);

        CubeSpawner.Instance.SpawnCubeInField(newCubeLevel, newCubePosition, newCubeRotation, isTakeImpulse: true);
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

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent(out CubeMerger cubeMerger))
            TryMergeCube(cubeMerger);
    }
    
    private void SaveLastMergeTime() => _lastMergeTime = Time.time;

    private void OnDrawGizmos()
    {
        if(Time.time < _lastMergeTime + MinMergeTimeInterval)
            Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
