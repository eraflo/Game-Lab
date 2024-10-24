

using UnityEngine;

namespace Ex3
{
    public interface IOrbit
    {
        Vector3 Orbit(float angle);
        float Radius { get; set; }
        float Distance { get; set; }
        OrbitDirection Direction { get; set; }

        bool ToggleOrbit { get; set; }
    }

    public enum OrbitDirection
    {
        Clockwise,
        CounterClockwise
    }

    public enum OrbitType
    {
        Circular,
        Elliptical
    }
}