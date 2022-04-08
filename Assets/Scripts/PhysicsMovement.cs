using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PhysicsMovement : MonoBehaviour
{
    public const float MinImpulse = 0f;
    public const float MaxImpulse = 25f;

    private Rigidbody _rigidbody;

    public void AddImpulse(Vector3 direction, float impulseFrom0To1)
    {
        Mathf.Clamp01(impulseFrom0To1);
        direction = direction.normalized;
        _rigidbody.AddForce(direction * Mathf.Lerp(MinImpulse, MaxImpulse, impulseFrom0To1), ForceMode.Impulse);
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
