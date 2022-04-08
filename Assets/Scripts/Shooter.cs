using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shooter : MonoBehaviour
{
    private const float GizmosLineHeight = 1.5f;

    [SerializeField] private Gun _gun;
    [SerializeField] private float _xRange = 4f; 
    [SerializeField] private float _loadingTime = 0.8f;
    public float XRange => _xRange;
    public Vector3 RightEdgePosition => transform.position + transform.right * MathHelper.Half(_xRange);
    public Vector3 LeftEdgePosition => transform.position - transform.right * MathHelper.Half(_xRange);

    public int MaxCubeLevel = Cube.StartLevel;
    private Vector3 _startPosition;
    private float _anchorXPosition = 0f;
    private float _currentXPosition = 0f;


    public void Shoot()
    {
        _gun.Shoot();
        StartCoroutine(LoadAfterSeconds(_loadingTime));
    }

    public void SaveAnchorPosition()
    {
        _anchorXPosition = _currentXPosition;
    }

    public void MoveGun(float offsetXPosition)
    {
        MoveGunToXPosition(_anchorXPosition + offsetXPosition);
    }

    protected virtual int CubeLevel(int maxCubeLevel)
    {
        int randomValue = Random.Range(Cube.LevelToNumber(Cube.StartLevel - 1), Cube.LevelToNumber(maxCubeLevel));
        int invertedLevel = Cube.NumberToLevel(randomValue);
        return maxCubeLevel - invertedLevel;
    }

    private IEnumerator LoadAfterSeconds(float time)
    {
        yield return new WaitForSeconds(time);
        Load();
    }

    private void Load()
    {
        _gun.Load(CubeLevel(MaxCubeLevel));
    }

    private void Awake()
    {
        _startPosition = _gun.transform.position;
        Load();
    }

    private void MoveGunToXPosition(float xPosition)
    {
        xPosition = Mathf.Clamp(xPosition, -MathHelper.Half(_xRange), MathHelper.Half(_xRange));
        _currentXPosition = xPosition;
        _gun.transform.position = _startPosition + xPosition * transform.right;
    }
}
