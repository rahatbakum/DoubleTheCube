using UnityEngine;

public class Gun : MonoBehaviour
{
    private const float DefaultImpulseFrom0To1 = 1f;

    private GunState _state = GunState.NotLoading;
    private Cube _cube;

    public void Load(int level = 1)
    {
        if(_state != GunState.NotLoading)
            return;
        _cube = CubeSpawner.Instance.SpawnCubeOutsideField(level, transform.position, transform.rotation);
        _cube.transform.SetParent(transform);
        _state = GunState.Loading;
    }

    public void Shoot()
    {
        if(_state != GunState.Loading)
            return;
        CubeSpawner.Instance.AddCubeToList(_cube);
        _cube.GetComponent<PhysicsMovement>().AddImpulse(transform.forward, DefaultImpulseFrom0To1);
        _state = GunState.NotLoading;
    }

    private enum GunState
    {
        NotLoading,
        Loading
    }
}
