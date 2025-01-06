using UnityEngine;

namespace Ex3
{
    public class Elliptical : IOrbit
    {
        public float Radius { get; set; } = 1;
        public OrbitDirection Direction { get; set; } = OrbitDirection.Clockwise;
        public bool ToggleOrbit { get; set; } = false;

        public Vector3 Orbit(float angle, Transform parent, Vector3 rotationAxis)
        {
            Vector3 parentPos = parent.position;
            float a = Radius;
            float b = Radius * 0.5f;

            // Calculate the direction of the orbit
            float direction = Direction == OrbitDirection.Clockwise ? 1 : -1;

            // Calculate the angle in radians
            angle *= Mathf.Deg2Rad;
            angle *= direction;

            // Normalize the rotation axis
            Vector3 rotationAxisNormalize = rotationAxis.normalized;

            // Calculate the old coordinates based on the ellipse equation
            float x0 = a * Mathf.Cos(angle);
            float z0 = b * Mathf.Sin(angle);

            // Calculate the rotation quaternion needed to rotate the point on the plane perpendicular to the rotation axis
            Vector3 perpendicularAxis = Vector3.Cross(rotationAxisNormalize, Vector3.up);
            float theta = Mathf.Acos(Vector3.Dot(perpendicularAxis, Vector3.right));

            float x = Mathf.Sin(theta / 2) * perpendicularAxis.x;
            float y = Mathf.Sin(theta / 2) * perpendicularAxis.y;
            float z = Mathf.Sin(theta / 2) * perpendicularAxis.z;

            Quaternion rotationPerpendicular = new Quaternion(x, y, z, Mathf.Cos(theta / 2));
            Quaternion inverse = Quaternion.Inverse(rotationPerpendicular);

            // Calculate Quaternion of the point
            Quaternion centeredPointQ = new Quaternion(x0, 0, z0, 0);

            // Rotate the point
            Quaternion rotatedPointQ = rotationPerpendicular * centeredPointQ * inverse;

            Vector3 point = new Vector3(rotatedPointQ.x, rotatedPointQ.y, rotatedPointQ.z);

            return point;
        }
    }
}