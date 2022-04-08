using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _impulse = 20f;
    [SerializeField] private float _angle = 45f;
    [SerializeField] private float _sinAngle = 0f;
    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);

            
            
            float d = _target.transform.position.x - transform.position.x;
            _sinAngle = (-Physics.gravity.y) * d / _impulse / _impulse;
            _angle = Mathf.Asin(_sinAngle) / 2 * Mathf.Rad2Deg;
            Vector3 velocity = Quaternion.Euler(0f, 0f, _angle) * Vector2.right * _impulse;
            GameObject sphere = Instantiate(_prefab, transform.position, transform.rotation, transform) as GameObject;
            sphere.GetComponent<Rigidbody>().velocity = velocity;
            Destroy(sphere, 5f);
        }
    }

    private void OnDrawGizmos()
    {
        float rads = _angle / Mathf.Rad2Deg;
        float offset = _impulse * _impulse * Mathf.Sin(2 * rads) / (- Physics.gravity.y);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * offset);
    }
}
