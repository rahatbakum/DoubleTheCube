using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawnerTester : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SpawnCubes());
    }

    IEnumerator SpawnCubes()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            CubeSpawner.Instance.SpawnCube(Random.Range(1,10), transform.position, transform.rotation);
        }
    }
}
