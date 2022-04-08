using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Shooter : MonoBehaviour
{
    private const float GizmosLineHeight = 1.5f;

    [SerializeField] private Gun _gun;
    [SerializeField] private float _xRange = 4f; 

    public int MaxCubeLevel = 10;

    public void Load()
    {
        _gun.Load(CubeLevel(MaxCubeLevel));
    }

    public void Shoot()
    {
        _gun.Shoot();
    }

    protected virtual int CubeLevel(int maxCubeLevel)
    {
        int randomValue = Random.Range(Cube.LevelToNumber(Cube.StartLevel), Cube.LevelToNumber(maxCubeLevel));
        int invertedLevel = Cube.NumberToLevel(randomValue);
        return maxCubeLevel - invertedLevel;
    }
}
