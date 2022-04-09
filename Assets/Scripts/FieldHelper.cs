using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldHelper
{
    ///<summary>
    ///Returns impulse direction for throwing from first position to second. Ignore y positions.  
    ///</summary>
    public static Vector3 ImpulseDirectionByValue(float impulseValue, Vector3 position1, Vector3 position2, ThrowMode throwMethod = ThrowMode.High)
    {
        Vector3 position2WithPosition1Y = new Vector3(position2.x, position1.y, position2.z);
        float distance = Vector3.Distance(position1, position2WithPosition1Y);
        float sinOfAngle = (-Physics.gravity.y) * distance / impulseValue / impulseValue;
        sinOfAngle = Mathf.Clamp(sinOfAngle, -1f, 1f);
        
        float doubleAngle = Mathf.Asin(sinOfAngle) * Mathf.Rad2Deg;
        if(throwMethod == ThrowMode.Low)
            doubleAngle = Mathf.Min(doubleAngle, MathHelper.HalfOfCircleDegrees - doubleAngle);
        else
            doubleAngle = Mathf.Max(doubleAngle, MathHelper.HalfOfCircleDegrees - doubleAngle);
        float angle = MathHelper.Half(doubleAngle);

        Vector3 direction = position2WithPosition1Y - position1;
        Quaternion zRotation = Quaternion.Euler(0f, 0f, angle);
        Quaternion yRotation = Quaternion.FromToRotation(Vector3.right, direction);
        Vector3 impulse = yRotation * zRotation * Vector3.right;
        return impulse;
    }

    public static Cube NearestCubeByLevel(List<Cube> cubes, int level, Vector3 position) // returns null when there is no cube with same level
    {
        List<Cube> cubesWithThisLevel = new List<Cube>();
        foreach(var item in cubes)
        {
            if(item.Level == level)
                cubesWithThisLevel.Add(item);
        }

        if(cubesWithThisLevel.Count == 0)
            return null;

        float nearestDistance = float.MaxValue;
        Cube nearestCube = cubesWithThisLevel[0];
        foreach(var item in cubesWithThisLevel)
        {
            float distance = Vector3.Distance(position, item.transform.position);
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestCube = item;  
            }
        }
        return nearestCube;
    }

    public static Vector3 DefaultImpulseDirection() => new Vector3(0f, 1f, 0.1f).normalized;
}

public enum ThrowMode
{
    Low,
    High
}