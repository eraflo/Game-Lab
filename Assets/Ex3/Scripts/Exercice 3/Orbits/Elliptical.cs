using UnityEngine;

namespace Ex3
{
    public class Elliptical : IOrbit
    {
        public float Radius { get; set; }
        public float Distance { get; set; }
        public OrbitDirection Direction { get; set; }
        public bool ToggleOrbit { get; set; } = false;

        public Vector3 Orbit(float angle)
        {
            float a = Radius;
            float b = Radius * 0.5f;

            float x = a * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = b * Mathf.Sin(angle * Mathf.Deg2Rad);

            return new Vector3(x, 0, z);
        }
    }
}