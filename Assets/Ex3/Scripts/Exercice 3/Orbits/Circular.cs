using UnityEngine;

namespace Ex3
{
    public class Circular : IOrbit
    {
        public float Radius { get; set; }
        public float Distance { get; set; }
        public OrbitDirection Direction { get; set; }
        public bool ToggleOrbit { get; set; } = false;

        public Vector3 Orbit(float angle)
        {
            float x = Radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            float z = Radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            return new Vector3(x, 0, z);
        }
    }
}