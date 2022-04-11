using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class Shooter : MonoBehaviour
{
    private const int MaxCubeLevelOffset = -2;
    private const float GizmosLineHeight = 1.5f;

    [SerializeField] private Gun _gun;
    [SerializeField] private CubeSpawner _cubeSpawner;
    [Min(MathHelper.MinNotZeroNumber)]
    [SerializeField] private float _xRange = 4f; 
    [Min(MathHelper.MinNotZeroNumber)]
    [SerializeField] private float _loadingTime = 0.2f;

    [SerializeField] private UnityEvent<Cube> _loaded;
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

    public float XRange => _xRange;
    public Vector3 RightEdgePosition => transform.position + transform.right * MathHelper.Half(_xRange);
    public Vector3 LeftEdgePosition => transform.position - transform.right * MathHelper.Half(_xRange);

    private int _maxCubeLevel = Cube.StartLevel;
    private Vector3 _startPosition;
    private float _anchorXPosition = 0f;
    private float _currentXPosition = 0f;
    private bool _isSavedAnchorXPosition = false;
    private bool _isWaitingForLoad = false;

    public void Shoot()
    {
        _isSavedAnchorXPosition = false;
        _gun.Shoot();
        if(_isWaitingForLoad)
            return;
        StartCoroutine(LoadAfterSeconds(_loadingTime));
    }

    public void SaveAnchorPosition()
    {
        _anchorXPosition = _currentXPosition;
        _isSavedAnchorXPosition = true;
    }

    public void MoveGun(float offsetXPosition)
    {
        if(!_isSavedAnchorXPosition)
            return;
        MoveGunToXPosition(_anchorXPosition + offsetXPosition);
    }

    protected virtual int CubeLevel(int maxCubeLevel)
    {
        int randomValue = Random.Range(Cube.LevelToNumber(Cube.StartLevel - 1), Cube.LevelToNumber(maxCubeLevel));
        int invertedLevel = Cube.NumberToLevel(randomValue);
        return maxCubeLevel - invertedLevel;
    }

    private void OnShot(Cube cube) => _shot.Invoke(cube);
    private void OnLoaded(Cube cube) => _loaded.Invoke(cube);

    private void OnCubeSpawnedAfterMerge(Cube cube)
    {
        int newMaxCubeLevel = cube.Level + MaxCubeLevelOffset;
        if(_maxCubeLevel >= newMaxCubeLevel)
            return;
        if(newMaxCubeLevel < Cube.StartLevel)
            _maxCubeLevel = Cube.StartLevel;
        else
            _maxCubeLevel = newMaxCubeLevel;
        return;
    }

    private IEnumerator LoadAfterSeconds(float time)
    {
        _isWaitingForLoad = true;
        yield return new WaitForSeconds(time);
        _isWaitingForLoad = false;
        Load();
    }

    private void Load()
    {
        _gun.Load(CubeLevel(_maxCubeLevel));
    }

    private void Awake()
    {
        _startPosition = _gun.transform.position;
        _gun.Initialize(_cubeSpawner);
        Load();
    }

    private void OnEnable()
    {   
        _cubeSpawner.CubeSpawnedAfterMerge += OnCubeSpawnedAfterMerge;
        _gun.Loaded += OnLoaded;
        _gun.Shot += OnShot;
    }

    private void OnDisable()
    {   
        _cubeSpawner.CubeSpawnedAfterMerge -= OnCubeSpawnedAfterMerge;
        _gun.Loaded -= OnLoaded;
        _gun.Shot -= OnShot;
    }

    private void MoveGunToXPosition(float xPosition)
    {
        xPosition = Mathf.Clamp(xPosition, -MathHelper.Half(_xRange), MathHelper.Half(_xRange));
        _currentXPosition = xPosition;
        _gun.transform.position = _startPosition + xPosition * transform.right;
    }
}
