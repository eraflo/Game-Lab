

using UnityEngine;

namespace Ex3
{
    public class BaseSystemElement : MonoBehaviour, ISystemElement
    {
        [SerializeField]
        BaseSystemElement revolvedPlanet;

        [SerializeField]
        private SystemElementType type;


        public SystemElementType Type { get; set; }
        public IOrbit Orbit { get; set; }
        public ISystemElement RevolvedPlanet { get => revolvedPlanet; set => revolvedPlanet = (BaseSystemElement)value; }

        public string Name { get; set; } = string.Empty;
        public float Speed { get; set; } = 1;
        public float RotationSpeed { get; set; } = 1;
        public Color Color { get; set; } = Color.black;
        public float Radius { get; set; } = 1;
        public float Distance { get; set; } = 1;
        public Transform ElemTransform { get; private set; }

        private void Start()
        {
            ElemTransform = GetComponent<Transform>();
            RevolvedPlanet = revolvedPlanet;
        }

        private void Update()
        {
            GetComponent<Renderer>().material.color = Color;

            transform.localScale = new Vector3(Radius, Radius, Radius);
        }

        public void Rotate()
        {
            ElemTransform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
        }

        public void Revolve()
        {
            float angle = Time.time * 360 / Orbit.Distance * Speed;
            transform.position = RevolvedPlanet.ElemTransform.position + Orbit.Orbit(angle);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;

            // Draw orbit
            if (Orbit != null && Orbit.ToggleOrbit)
            {
                Vector3 lastPos = RevolvedPlanet.ElemTransform.position + Orbit.Orbit(0);
                for (int i = 1; i <= 360; i++)
                {
                    Vector3 pos = RevolvedPlanet.ElemTransform.position + Orbit.Orbit(i);
                    Gizmos.DrawLine(lastPos, pos);
                    lastPos = pos;
                }
            }
        }
    }
}