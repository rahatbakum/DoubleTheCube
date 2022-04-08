using UnityEngine;

[DisallowMultipleComponent]
public class CubeMerger : MonoBehaviour
{
    [SerializeField] private Cube _cube;
    private bool isMerged = false;

    public bool TryMergeCube(CubeMerger cubeMerger)
    {   
        if(isMerged)
            return false;
        if(_cube == null || cubeMerger._cube == null)
            return false;
        if(!IsSuitableCubes(_cube,cubeMerger._cube))
            return false;
        if(CubeSpawner.Instance == null)
            return false;

        isMerged = true;
        cubeMerger.isMerged = true;
        Cube slowerCube = SlowerCube(_cube, cubeMerger._cube);
        int newCubeLevel = NewCubeLevel(_cube.Level, cubeMerger._cube.Level);
        float impulse = Impulse(_cube, cubeMerger._cube);
        Vector3 newCubePosition = slowerCube.transform.position;
        Quaternion newCubeRotation = slowerCube.transform.rotation;

        CubeSpawner.Instance.DestroyCube(_cube);
        CubeSpawner.Instance.DestroyCube(cubeMerger._cube);

        CubeSpawner.Instance.SpawnCubeInField(newCubeLevel, newCubePosition, newCubeRotation, isTakeImpulse: true);
        return true;
    }

    private static int NewCubeLevel(int cube1Level, int cube2Level) => cube1Level + 1;

    private static float Impulse(Cube cube1, Cube cube2)
    {
        float cube1Velocity;
        float cube2Velocity;
        if(cube1.TryGetComponent<Rigidbody>(out Rigidbody cube1RigidBody))
            cube1Velocity = cube1RigidBody.velocity.magnitude;
        else
            cube1Velocity = 0f;
        if(!cube2.TryGetComponent<Rigidbody>(out Rigidbody cube2RigidBody))
            cube2Velocity = cube2RigidBody.velocity.magnitude;
        else
            cube2Velocity = 0f;

        return cube1Velocity + cube2Velocity;
    }

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
}
