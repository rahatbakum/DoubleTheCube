using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shooter : MonoBehaviour
{
    private const int MaxCubeLevelOffset = -2;
    private const float GizmosLineHeight = 1.5f;

    [SerializeField] private Gun _gun;
    [SerializeField] private float _xRange = 4f; 
    [SerializeField] private float _loadingTime = 0.2f;
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
        if(_gun.State != GunState.Loading)
            return;
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

    private void OnCubeSpawned(Cube cube)
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
        Load();
    }

    private void OnEnable()
    {   
        if(CubeSpawner.Instance != null)
            CubeSpawner.Instance.CubeSpawned += OnCubeSpawned;
    }

    private void OnDisable()
    {   
        if(CubeSpawner.Instance != null)
            CubeSpawner.Instance.CubeSpawned -= OnCubeSpawned;
    }

    private void MoveGunToXPosition(float xPosition)
    {
        xPosition = Mathf.Clamp(xPosition, -MathHelper.Half(_xRange), MathHelper.Half(_xRange));
        _currentXPosition = xPosition;
        _gun.transform.position = _startPosition + xPosition * transform.right;
    }
}
