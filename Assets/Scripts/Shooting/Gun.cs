using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour
{
    private const float DefaultVelocityValueFrom0To1 = 1f;

    [SerializeField] private UnityEvent<Cube> _loaded;
    private CubeSpawner _cubeSpawner;

    public event UnityAction<Cube> Loaded
    {
        add => _loaded.AddListener(value);
        remove => _loaded.RemoveListener(value);
    }
    [SerializeField] private UnityEvent<Cube> _shot;
    public event UnityAction<Cube> Shot
    {
        add => _shot.AddListener(value);
        remove => _shot.RemoveListener(value);
    }

    private GunState _state = GunState.NotLoading;
    public GunState State => _state;
    private Cube _cube;

    public void Load(int level = 1)
    {
        if(_state != GunState.NotLoading)
            return;
        _state = GunState.Loading;
        _cube = _cubeSpawner.SpawnCubeOutsideField(level, transform.position, transform.rotation);
        _cube.transform.SetParent(transform);
        _loaded.Invoke(_cube);
    }

    public void Shoot()
    {
        if(_state != GunState.Loading)
            return;
        _state = GunState.NotLoading;
        _cubeSpawner.AddCubeToList(_cube);
        _cube.GetComponent<PhysicsMovement>().AddVelocity(transform.forward, DefaultVelocityValueFrom0To1);
        _shot.Invoke(_cube);
    }

    public void Initialize(CubeSpawner cubeSpawner)
    {
        _cubeSpawner = cubeSpawner;
    }
}

public enum GunState
{
    NotLoading,
    Loading
}
