using UnityEngine;

public class Gun : MonoBehaviour
{
    private const float DefaultImpulseFrom0To1 = 1f;

    private GunState _state = GunState.NotLoading;
    public GunState State => _state;
    private Cube _cube;

    public void Load(int level = 1)
    {
        if(_state != GunState.NotLoading)
            return;
        _state = GunState.Loading;
        _cube = CubeSpawner.Instance.SpawnCubeOutsideField(level, transform.position, transform.rotation);
        _cube.transform.SetParent(transform);
    }

    public void Shoot()
    {
        if(_state != GunState.Loading)
            return;
        _state = GunState.NotLoading;
        CubeSpawner.Instance.AddCubeToList(_cube);
        _cube.GetComponent<PhysicsMovement>().AddImpulse(transform.forward, DefaultImpulseFrom0To1);
        
    }
}

public enum GunState
{
    NotLoading,
    Loading
}
