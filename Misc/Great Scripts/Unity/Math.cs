
// Static Math Functions Script
// by Kyle Furey

using System.Collections.Generic;
using UnityEngine;

// New math functions.
public static class Math
{
    // VECTOR

    // Returns the squared distance between two vectors
    public static float DistanceSquared(Vector3 pointA, Vector3 pointB)
    {
        float xDistance = pointA.x - pointB.x;
        float yDistance = pointA.y - pointB.y;
        float zDistance = pointA.z - pointB.z;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }

    // Returns an offset vector based on the relative transform and given offset vector
    public static Vector3 TranslateRelative(Transform transform, Vector3 offset)
    {
        Vector3 directionX = transform.right * offset.x;
        Vector3 directionY = transform.up * offset.y;
        Vector3 directionZ = transform.forward * offset.z;

        return transform.position + directionX + directionY + directionZ;
    }

    // Returns the direction between two vectors
    public static Vector3 Direction(Vector3 pointA, Vector3 pointB)
    {
        return pointB - pointA;
    }

    // Returns the cross product of a vector
    public static float CrossProduct(Vector3 pointA, Vector3 pointB, Vector3 pointC, bool useY = false)
    {
        return (pointB.x - pointA.x) * (useY ? (pointC.y - pointA.y) : (pointC.z - pointA.z)) - (pointC.x - pointA.x) * (useY ? (pointB.y - pointA.y) : (pointB.z - pointA.z));
    }

    // Rotate a vector by a euler rotation
    public static Vector3 Rotate(Vector3 direction, Vector3 rotation)
    {
        return Quaternion.Euler(rotation) * direction;
    }

    // Rotate a vector by a quaternion rotation
    public static Vector3 Rotate(Vector3 direction, Quaternion rotation)
    {
        return rotation * direction;
    }

    // Moves a vector towards another vector by a linear speed (without overshooting)
    public static Vector3 MoveTowards(Vector3 start, Vector3 end, float speed)
    {
        return (DistanceSquared(start, end) <= speed * speed) ? (end) : (start + ((end - start).normalized * speed));
    }

    // Moves a vector towards another vector by a percentage of the distance to the destination (and reaches the target after a certain distance is met)
    public static Vector3 LinearInterpolate(Vector3 start, Vector3 end, float alpha, float minimumDistance = 0)
    {
        return (DistanceSquared(start, end) <= minimumDistance * minimumDistance) ? (end) : (start + ((end - start) * alpha));
    }

    // Gets the bezier interpolation of two points based on a third point
    public static Vector3 BezierInterpolate(Vector3 start, Vector3 end, Vector3 curve, float alpha)
    {
        return LinearInterpolate(LinearInterpolate(start, curve, alpha, 0), LinearInterpolate(curve, end, alpha, 0), alpha, 0);
    }

    // Returns a list of vectors representing a bezier curve
    public static List<Vector3[]> BezierCurve(Vector3 start, Vector3 end, Vector3 curve, int resolution)
    {
        List<Vector3[]> result = new List<Vector3[]>(resolution);

        Vector3 previous = start;

        for (int i = 0; i < resolution; i++)
        {
            float alpha = (i + 1) / resolution;

            Vector3 next = BezierInterpolate(start, end, curve, alpha);

            Vector3[] line = new Vector3[2]
            {
                previous, next
            };

            result.Add(line);

            previous = next;
        }

        return result;
    }


    // FLOAT

    // Returns a percentage relative to a value of a minimum and maximum
    public static float Percentage(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    // Returns a value relative to a percentage of a minimum and maximum
    public static float Value(float percentage, float min, float max)
    {
        return (max - min) * percentage + min;
    }

    // Move a value towards another value by a linear speed (without overshooting)
    public static float MoveTowards(float start, float end, float speed)
    {
        return (Mathf.Abs(end - start) <= speed) ? (end) : ((start > end) ? (start + speed) : (start - speed));
    }

    // Moves a value towards another value by a percentage of the distance to the destination (and reaches the target after a certain distance is met)
    public static float LinearInterpolate(float start, float end, float alpha, float minimumDistance = 0)
    {
        return (Mathf.Abs(end - start) <= minimumDistance) ? (end) : (start + ((end - start) * alpha));
    }
}
