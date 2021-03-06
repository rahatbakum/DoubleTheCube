using UnityEngine;

public static class MathHelper
{
    public const float FullCircleDegrees = 360f;
    public const float HalfOfCircleDegrees = FullCircleDegrees * HalfOne; 
    public const float HalfOne = 0.5f;
    public const float MinNotZeroNumber = 0.01f;

    public static float Half(float value) => value * HalfOne; 
    public static Vector3 Half(Vector3 value) => value * HalfOne;    
}
