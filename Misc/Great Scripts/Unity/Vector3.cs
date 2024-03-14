
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

    // Returns the cross product of a vector 3
    public static float CrossProduct(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        return (pointB.x - pointA.x) * (pointC.y - pointA.y) - (pointC.x - pointA.x) * (pointB.y - pointA.y);
    }

    // Get a normal from 3 points
    public static Vector3 GetNormal(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Plane plane = new Plane(pointA, pointB, pointC);

        return plane.normal;
    }

    // Get a normal from an array of 3 points
    public static Vector3 GetNormal(Vector3[] points)
    {
        Plane plane = new Plane(points[0], points[1], points[2]);

        return plane.normal;
    }

    // Get a normal from a triangle
    public static Vector3 GetNormal(Mesh triangle)
    {
        Plane plane = new Plane(triangle.vertices[0], triangle.vertices[1], triangle.vertices[2]);

        return plane.normal;
    }

    // Get a normal from a color
    public static Vector3 GetNormal(Color color)
    {
        return new Vector3(color.r, color.g, color.b);
    }

    // Returns whether the new point is convex relative to a given mesh
    private static bool IsConvex(Vector3[] vertices, int[] triangles)
    {
        // Loop through triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Make triangle
            Plane triangle = new Plane(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);

            // Loop through points
            for (int j = 0; j < vertices.Length; j++)
            {
                // Ignore the plane's points
                if (j == i || j == i + 1 || j == i + 2)
                {
                    continue;
                }

                // Check that the point is not outside the triangle
                if (triangle.GetSide(vertices[j]))
                {
                    // A point is concave
                    return false;
                }
            }
        }

        // All points are convex
        return true;
    }

    // Returns whether the new point is convex relative to a given mesh
    private static bool IsConvex(Mesh mesh)
    {
        return IsConvex(mesh.vertices, mesh.triangles);
    }

    // Returns whether the new point is concave relative to a given mesh
    private static bool IsConcave(Vector3[] vertices, int[] triangles)
    {
        // Loop through triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            // Make triangle
            Plane triangle = new Plane(vertices[triangles[i]], vertices[triangles[i + 1]], vertices[triangles[i + 2]]);

            // Loop through points
            for (int j = 0; j < vertices.Length; j++)
            {
                // Ignore the plane's points
                if (j == i || j == i + 1 || j == i + 2)
                {
                    continue;
                }

                // Check that the point is not outside the triangle
                if (triangle.GetSide(vertices[j]))
                {
                    // A point is concave
                    return true;
                }
            }
        }

        // All points are convex
        return false;
    }

    // Returns whether the new point is concave relative to a given mesh
    private static bool IsConcave(Mesh mesh)
    {
        return IsConcave(mesh.vertices, mesh.triangles);
    }
}
