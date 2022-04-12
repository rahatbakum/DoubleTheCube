using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZVelocityLimiter : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;
    [SerializeField] private float _maxZVelocity = 1.75f;

    private void FixedUpdate()
    {
        LimitZVelocity();
    }

    private void LimitZVelocity()
    {
        if(_rigidBody.velocity.z < -_maxZVelocity)
            _rigidBody.velocity = new Vector3(_rigidBody.velocity.x, _rigidBody.velocity.y, -_maxZVelocity);
    }
}
