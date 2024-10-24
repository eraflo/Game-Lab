

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
            float angle = RotationSpeed * Time.deltaTime;
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
            //transform.position = RevolvedPlanet.ElemTransform.position + Orbit.Orbit(angle, RevolvedPlanet.RotationAxis);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color;

            // Draw orbit
            if (Orbit != null && Orbit.ToggleOrbit)
            {
                //Vector3 lastPos = RevolvedPlanet.ElemTransform.position + Orbit.Orbit(0, RevolvedPlanet.RotationAxis);
                for (int i = 1; i <= 360; i++)
                {
                    //Vector3 pos = RevolvedPlanet.ElemTransform.position + Orbit.Orbit(i, RevolvedPlanet.RotationAxis);
                    //Gizmos.DrawLine(lastPos, pos);
                    //lastPos = pos;
                }
            }

            // Draw rotation axis
            Gizmos.color = Color.red;

            ElemTransform = transform;

            Gizmos.DrawLine(ElemTransform.position - RotationAxis, ElemTransform.position + RotationAxis);
        }
    }
}