using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework.Math
{
    /// <summary>
    /// Extension for math library.
    /// </summary>
    public static class MathfExtension
    {
        /// <summary>
        /// Returns the determinant of a matrix whose rows are the two specified vectors.
        /// </summary>
        /// <param name="vector1">The values of the first row.</param>
        /// <param name="vector2">The values of the second row.</param>
        public static float GetDeterminant(Vector2 vector1, Vector2 vector2)
        {
            return vector1.x * vector2.y - vector2.x * vector1.y;
        }

        /// <summary>
        /// Returns the squared distance from a point to a line segment.
        /// </summary>
        /// <param name="segmentStart">The first end of the line segment.</param>
        /// <param name="segmentEnd">The second end of the line segment.</param>
        /// <param name="point">The point to which the squared distance is to be calculated.</param>
        /// <returns></returns>
        public static float GetSquaredDistanceToSegment(Vector2 segmentStart, Vector2 segmentEnd, Vector2 point)
        {
            // The t value dertermines the projection of the point onto the line segment
            // + t < 0: The projection is outside the segment and close to segmentStart.
            // + t > 1: The projection is outside the segment and close to segmentEnd.
            // + 0 <= t <= 1: The projection is on the line segment.
            float t = Vector3.Dot(point - segmentStart, segmentEnd - segmentStart) / (segmentEnd - segmentStart).sqrMagnitude;

            if (t < 0.0f)
            {
                return Vector2.Distance(point, segmentStart);
            }

            if (t > 1.0f)
            {
                return Vector2.Distance(point, segmentEnd);
            }

            Vector2 projection = segmentStart + t * (segmentEnd - segmentStart);
            return (point - projection).sqrMagnitude;
        }

        /// <summary>
        /// Returns the signed distance from a point to a line. 
        /// If the distance is positive, then the point is on the left of the line in the Cartesian coordinate.
        /// </summary>
        /// <param name="linePoint1">The first point of the line.</param>
        /// <param name="linePoint2">The second point of the line.</param>
        /// <param name="point">The point to which the signed distance is to be calculated.</param>
        /// <returns></returns>
        public static float GetSignedDistance(Vector2 linePoint1, Vector2 linePoint2, Vector2 point)
        {
            return GetDeterminant(linePoint1 - point, linePoint2 - linePoint1);
        }

        // https://stackoverflow.com/questions/37499652/calculate-point-on-a-circle-in-3d-space
        public static Vector3 GetRandomPointOnCircle(Vector3 center, float radius, Vector3 normal)
        {
            normal = normal.normalized;

            Vector3 firstBasisVector;
            Vector3 secondBasisVector;
            if (normal == Vector3.up || normal == Vector3.down)
            {
                firstBasisVector = Vector3.right;
                secondBasisVector = Vector3.forward;
            }
            else
            {
                firstBasisVector = Vector3.Cross(normal, Vector3.up).normalized;
                secondBasisVector = Vector3.Cross(firstBasisVector, normal).normalized;
            }

            float angle = Random.Range(0.0f, 2 * Mathf.PI);

            return center + radius * (firstBasisVector * Mathf.Cos(angle) + secondBasisVector * Mathf.Sin(angle));
        }

        // Sphere point picking: https://mathworld.wolfram.com/SpherePointPicking.html
        public static Vector3 GetRandomPointOnSphere(Vector3 center, float radius)
        {
            float x = Random.Range(1.0f, 5.0f);
            float y = Random.Range(1.0f, 5.0f);
            float z = Random.Range(1.0f, 5.0f);

            float normalizeValue = Mathf.Sqrt(x * x + y * y + z * z);

            return center + new Vector3(x / normalizeValue * radius, y / normalizeValue * radius, z / normalizeValue * radius);
        }

        // Get a random unit vector in a cone.
        public static Vector3 GetRandomUnitVectorInCone(Vector3 coneDirection, float coneHalfAngleRad)
        {
            // Normalize the cone direction, in case it is not a unit vector.
            coneDirection = coneDirection.normalized;

            // If the half angle is 0, return the cone direction.
            if (coneHalfAngleRad <= 0.0f)
            {
                return coneDirection;
            }
            else
            {
                // Get a random point on or in the cone's circle.
                float randomRadius = Random.Range(0.0f, coneDirection.magnitude * Mathf.Tan(coneHalfAngleRad));
                Vector3 randomPointOnCircle = GetRandomPointOnCircle(coneDirection, randomRadius, coneDirection);

                // From the random point found, calculate the random unit vector.
                Vector3 randomUnitVector = randomPointOnCircle.normalized;

                //Debug.Log($"Cone direction: {coneDirection}. Random direction: {randomUnitVector}. Angle: {Mathf.Deg2Rad * Vector3.Angle(coneDirection, randomUnitVector)}");

                return randomUnitVector;
            }
        }
    }
}
