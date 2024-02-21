
// Static Vector 3 Functions Script
// by Kyle Furey

using UnityEngine;

// New vector 3 functions.
public static class VectorThree
{
    // Returns the squared distance between two vector 3s
    public static float DistanceSquared(Vector3 pointA, Vector3 pointB)
    {
        float xDistance = pointA.x - pointB.x;
        float yDistance = pointA.y - pointB.y;
        float zDistance = pointA.z - pointB.z;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }

    // Returns the squared distance between two separated vector 3s
    public static float DistanceSquared(float pointAx, float pointAy, float pointAz, float pointBx, float pointBy, float pointBz)
    {
        float xDistance = pointAx - pointBx;
        float yDistance = pointAy - pointBy;
        float zDistance = pointAz - pointBz;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }

    // Returns an offset vector3 based on the relative transform and given offset vector 3
    public static Vector3 TranslateRelative(Transform transform, Vector3 offset)
    {
        Vector3 directionX = transform.right * offset.x;
        Vector3 directionY = transform.up * offset.y;
        Vector3 directionZ = transform.forward * offset.z;

        return transform.position + directionX + directionY + directionZ;
    }

    // Returns an offset vector3 based on the relative transform and given separated offset vector 3
    public static Vector3 TranslateRelative(Transform transform, float offsetX, float offsetY, float offsetZ)
    {
        Vector3 offset = new Vector3(offsetX, offsetY, offsetZ);

        Vector3 directionX = transform.right * offset.x;
        Vector3 directionY = transform.up * offset.y;
        Vector3 directionZ = transform.forward * offset.z;

        return transform.position + directionX + directionY + directionZ;
    }

    // Returns the direction between two vector 3s
    public static Vector3 Direction(Vector3 pointA, Vector3 pointB)
    {
        return pointB - pointA;
    }

    // Returns the direction between two separated vector 3s
    public static Vector3 Direction(float pointAx, float pointAy, float pointAz, float pointBx, float pointBy, float pointBz)
    {
        Vector3 pointA = new Vector3(pointAx, pointAy, pointAz);
        Vector3 pointB = new Vector3(pointBx, pointBy, pointBz);

        return pointA - pointB;
    }

    // Returns the cross product of a vector 3
    public static float CrossProduct(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        return (pointB.x - pointA.x) * (pointC.y - pointA.y) - (pointC.x - pointA.x) * (pointB.y - pointA.y);
    }

    // Returns a percentage relative to a value of a minimum and maximum
    private static float Percentage(float value, float min, float max)
    {
        return (value - min) / (max - min);
    }

    // Returns a value relative to a percentage of a minimum and maximum
    private static float Value(float percentage, float min, float max)
    {
        return (max - min) * percentage + min;
    }
}
