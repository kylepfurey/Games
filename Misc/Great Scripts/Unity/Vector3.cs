
// Static Vector 3 Functions Script
// Made for educational purposes.
// Copyright © 2024 by Kyle Furey

// New vector 3 functions.
public static class Vector3
{
    // Returns the squared distance between two vector 3s (for a more accurate distance calculation)
    public static float DistanceSquared(UnityEngine.Vector3 pointA, UnityEngine.Vector3 pointB)
    {
        float xDistance = pointA.x - pointB.x;
        float yDistance = pointA.y - pointB.y;
        float zDistance = pointA.z - pointB.z;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }

    // Returns the squared distance between two separated vector 3s (for a more accurate distance calculation)
    public static float DistanceSquared(float pointAx, float pointAy, float pointAz, float pointBx, float pointBy, float pointBz)
    {
        float xDistance = pointAx - pointBx;
        float yDistance = pointAy - pointBy;
        float zDistance = pointAz - pointBz;

        return xDistance * xDistance + yDistance * yDistance + zDistance * zDistance;
    }
}
