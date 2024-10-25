

using UnityEngine;

namespace Ex3
{
    public class BaseSystemElement : MonoBehaviour, ISystemElement
    {
        [SerializeField]
        BaseSystemElement revolvedPlanet;

        [SerializeField]
        private SystemElementType type;

        [SerializeField]
        private int segments = 100;


        public SystemElementType Type { get; set; }
        public IOrbit Orbit { get; set; }
        public ISystemElement RevolvedPlanet { get => revolvedPlanet; set => revolvedPlanet = (BaseSystemElement)value; }

        public string Name { get; set; } = string.Empty;
        public float Speed { get; set; } = 1;
        public float RotationSpeed { get; set; } = 1;
        public Color Color { get; set; } = Color.black;
        public float Radius { get; set; } = 1;
        public Transform ElemTransform { get; private set; }
        public Vector3 RotationAxis { get; set; } = Vector3.up;

        private Vector3 lastRotationAxis;


        private void Awake()
        {
            ElemTransform = transform;
            RevolvedPlanet = revolvedPlanet;
        }

        private void Update()
        {
            GetComponent<Renderer>().material.color = Color;

            transform.localScale = new Vector3(Radius, Radius, Radius);
        }

        public void Rotate()
        {
            // Do rotation with quaternion
            Vector3 rotationAxisNormalize = RotationAxis.normalized;
            float angle = RotationSpeed * Time.deltaTime * Mathf.Deg2Rad;
            float x = Mathf.Sin(angle / 2) * rotationAxisNormalize.x;
            float y = Mathf.Sin(angle / 2) * rotationAxisNormalize.y;
            float z = Mathf.Sin(angle / 2) * rotationAxisNormalize.z;

            Quaternion newRot = new Quaternion(x, y, z, Mathf.Cos(angle / 2));

            // Reset rotation if axis changed
            if (lastRotationAxis != RotationAxis)
            {
                ElemTransform.transform.rotation = Quaternion.identity;
                lastRotationAxis = RotationAxis;
            }

            ElemTransform.transform.rotation *= newRot;
        }

        public void Revolve()
        {
            float angle = Time.time * Speed;
            Vector3 parentPos = RevolvedPlanet.ElemTransform.position;
            transform.position = parentPos + Orbit.Orbit(angle, RevolvedPlanet.ElemTransform, RevolvedPlanet.RotationAxis);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;

            // Draw orbit
            if (Orbit != null && Orbit.ToggleOrbit)
            {
                Vector3 parentPos = RevolvedPlanet.ElemTransform.position;
                Vector3 lastPos = parentPos + Orbit.Orbit(0, RevolvedPlanet.ElemTransform, RevolvedPlanet.RotationAxis);

                for (int i = 1; i <= segments; i++)
                {
                    float theta = (float)i / segments * 360f;
                    Vector3 currentPoint = parentPos + Orbit.Orbit(theta, RevolvedPlanet.ElemTransform, RevolvedPlanet.RotationAxis);

                    Gizmos.DrawLine(lastPos, currentPoint);

                    lastPos = currentPoint;
                }
            }

            // Draw rotation axis
            Gizmos.color = Color.red;

            ElemTransform = transform;

            Gizmos.DrawLine(ElemTransform.position - RotationAxis, ElemTransform.position + RotationAxis);
        }
    }
}