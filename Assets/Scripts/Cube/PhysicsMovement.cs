using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PhysicsMovement : MonoBehaviour
{
    
    public const float MinTorqueValue = 0f;
    public const float MaxTorqueValue = 10f;
    public const float MinImpulseValue = 0f;
    public const float MaxImpulseValue = 25f;

    private Rigidbody _rigidbody;

    public void AddImpulse(Vector3 direction, float impulseValueFrom0To1, bool isAddTorque = false)
    {
        direction = direction.normalized;
        _rigidbody.AddForce(direction * RealImpulseValue(impulseValueFrom0To1), ForceMode.Impulse);
        if(isAddTorque)
            AddSmallRandomTorque();
    }

    public void AddSmallRandomTorque()
    {
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, MathHelper.FullCircleDegrees), Random.Range(0f, MathHelper.FullCircleDegrees), Random.Range(0f, MathHelper.FullCircleDegrees));
        Vector3 randomDirection = randomRotation * Vector3.right;
        Vector3 torque = randomDirection * Random.Range(MinTorqueValue, MaxTorqueValue);
        _rigidbody.AddTorque(torque, ForceMode.Impulse);
    }

    public static float RealImpulseValue(float impulseValueFrom0To1) => Mathf.Lerp(MinImpulseValue, MaxImpulseValue, Mathf.Clamp01(impulseValueFrom0To1));

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
