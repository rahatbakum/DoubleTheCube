using UnityEngine;

[RequireComponent(typeof (Rigidbody))]
public class PhysicsMovement : MonoBehaviour
{
    
    public const float MinTorqueValue = 0f;
    public const float MaxTorqueValue = 4f;
    public const float MinVelocityValue = 0f;
    public const float MaxVelocityValue = 30f;

    private Rigidbody _rigidbody;

    public void AddVelocity(Vector3 direction, float velocityValueFrom0To1, bool isAddTorque = false)
    {
        direction = direction.normalized;
        _rigidbody.AddForce(direction * RealVelocityValue(velocityValueFrom0To1), ForceMode.VelocityChange);
        if(isAddTorque)
            AddSmallRandomTorque();
    }

    public void AddSmallRandomTorque()
    {
        Quaternion randomRotation = Quaternion.Euler(Random.Range(0f, MathHelper.FullCircleDegrees), Random.Range(0f, MathHelper.FullCircleDegrees), Random.Range(0f, MathHelper.FullCircleDegrees));
        Vector3 randomDirection = randomRotation * Vector3.right;
        Vector3 torque = randomDirection * Random.Range(MinTorqueValue, MaxTorqueValue);
        _rigidbody.AddTorque(torque, ForceMode.VelocityChange);
    }

    public static float RealVelocityValue(float velocityValueFrom0To1) => Mathf.Lerp(MinVelocityValue, MaxVelocityValue, Mathf.Clamp01(velocityValueFrom0To1));

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
}
