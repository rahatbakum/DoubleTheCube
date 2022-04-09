using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooter : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _velocity = 20f;
    [SerializeField] private float _angle = 45f;
    [SerializeField] private float _sinAngle = 0f;
    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        
    }

    private IEnumerator Shoot()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.2f);

            
            float velocityValue = PhysicsMovement.MaxVelocityValue;
            Vector3 velocity = FieldHelper.VelocityDirectionByValue(velocityValue, transform.position, _target.position);
            GameObject sphere = Instantiate(_prefab, transform.position, transform.rotation, transform) as GameObject;
            sphere.GetComponent<Rigidbody>().AddForce(velocity * velocityValue, ForceMode.VelocityChange);
            Destroy(sphere, 2.5f);
        }
    }

    private void OnDrawGizmos()
    {
        float rads = _angle / Mathf.Rad2Deg;
        float offset = _velocity * _velocity * Mathf.Sin(2 * rads) / (- Physics.gravity.y);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * offset);
    }
}
