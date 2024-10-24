using UnityEngine;

namespace Ex3
{
    public class Elliptical : IOrbit
    {
        public float Radius { get; set; } = 1;
        public OrbitDirection Direction { get; set; } = OrbitDirection.Clockwise;
        public bool ToggleOrbit { get; set; } = false;

        public Vector3 Orbit(float angle, Vector3 initPos, Vector3 rotationAxis)
        {
            float a = Radius;
            float b = Radius * 0.5f;

            float direction = Direction == OrbitDirection.Clockwise ? 1 : -1;

            angle *= direction;
                       

            Vector3 rotationAxisNormalize = rotationAxis.normalized;
            float x = Mathf.Sin(angle / 2) * rotationAxisNormalize.x;
            float y = Mathf.Sin(angle / 2) * rotationAxisNormalize.y;
            float z = Mathf.Sin(angle / 2) * rotationAxisNormalize.z;

            Quaternion rotation = new Quaternion(x, y, z, Mathf.Cos(angle / 2));

            //Vector3 initPos = 

            return Vector3.zero;
        }
    }
}