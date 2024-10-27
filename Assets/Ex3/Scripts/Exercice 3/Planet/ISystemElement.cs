

using UnityEngine;

namespace Ex3
{
    public interface ISystemElement
    {
        void Rotate();

        void Revolve();

        SystemElementType Type { get; set; }

        IOrbit Orbit { get; set; }

        ISystemElement RevolvedPlanet { get; set; }

        string Name { get; set; }
        float RotationSpeed { get; set; }
        float Speed { get; set; }
        Color Color { get; set; }
        float Radius { get; set; }
        Transform ElemTransform { get; }
        Vector3 RotationAxis { get; set; }

        event SystemCollide OnSystemCollide;
    }

    public delegate void SystemCollide(ISystemElement element);

    public enum SystemElementType
    {
        Planet,
        Moon,
        Star
    }
}