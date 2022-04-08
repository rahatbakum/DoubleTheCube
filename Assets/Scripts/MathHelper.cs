public static class MathHelper
{
    public const float FullCircleDegrees = 360f;
    public const float HalfOfCircleDegrees = FullCircleDegrees * HalfOne; 
    public const float HalfOne = 0.5f;

    public static float Half(float value) => value * HalfOne;    
}
