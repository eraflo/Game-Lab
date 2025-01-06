

using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Ex3
{
    [RequireComponent(typeof(RectTransform))]
    public class SystemElementUI : MonoBehaviour
    {
        public delegate void OnClickDelegate(SystemElementUI element);
        public event OnClickDelegate OnClick;

        [SerializeField]
        private TMP_Text nameText;
        [SerializeField]
        private TMP_Text speedText;
        [SerializeField] 
        private TMP_Text rotationSpeedText;
        [SerializeField]
        private TMP_Text radiusText;

        [Header("Rotation Axis")]
        [SerializeField]
        private TMP_InputField x;
        [SerializeField]
        private TMP_InputField y;
        [SerializeField]
        private TMP_InputField z;

        [Header("System Type")]
        [SerializeField]
        private Material sunMat;
        [SerializeField]
        private Toggle isSun;

        [SerializeField]
        private Material moonMat;
        [SerializeField]
        private Toggle isMoon;

        private Material defaultMat;

        public ISystemElement systemElement { get; set; }

        public string NameText { get => nameText.text; set => nameText.text = value; }
        public string SpeedText { get => speedText.text; set => speedText.text = value; }
        public string RotationSpeedText { get => rotationSpeedText.text; set => rotationSpeedText.text = value; }
        public string RadiusText { get => radiusText.text; set => radiusText.text = value; }

        public string X { get => x.text; set => x.text = value; }
        public string Y { get => y.text; set => y.text = value; }
        public string Z { get => z.text; set => z.text = value; }

        public bool IsSun { get => isSun.isOn; set => isSun.isOn = value; }
        public bool IsMoon { get => isMoon.isOn; set => isMoon.isOn = value; }

        public bool HasOrbit { get => systemElement.Orbit != null; }

        private void Start()
        {
            defaultMat = systemElement.ElemTransform.gameObject.GetComponent<Renderer>().material;

            NameText = systemElement.Name;
            SpeedText = "Speed : " + systemElement.Speed.ToString();
            RotationSpeedText = "Rotation Speed : " + systemElement.RotationSpeed.ToString();
            RadiusText = "Radius : " + systemElement.Radius.ToString();

            X = systemElement.RotationAxis.x.ToString();
            Y = systemElement.RotationAxis.y.ToString();
            Z = systemElement.RotationAxis.z.ToString();

            x.onEndEdit.AddListener((string value) =>
            {
                systemElement.RotationAxis = new Vector3(float.Parse(X), float.Parse(Y), float.Parse(Z));
            });

            y.onEndEdit.AddListener((string value) =>
            {
                systemElement.RotationAxis = new Vector3(float.Parse(X), float.Parse(Y), float.Parse(Z));
            });

            z.onEndEdit.AddListener((string value) =>
            {
                systemElement.RotationAxis = new Vector3(float.Parse(X), float.Parse(Y), float.Parse(Z));
            });

            // Manage the sun and moon toggle

            isSun.onValueChanged.AddListener((value) =>
            {
                if (value)
                {
                    if(systemElement.ElemTransform.gameObject.GetComponent<Light>() == null)
                    {
                        Light light = systemElement.ElemTransform.gameObject.AddComponent<Light>();
                        light.color = Color.yellow;
                        light.type = LightType.Point;
                        light.intensity = 10 * systemElement.Radius;
                        light.range = 100 * systemElement.Radius;
                        systemElement.ElemTransform.gameObject.GetComponent<Renderer>().material = sunMat;
                    }
                    else
                    {
                        isMoon.isOn = false;
                    }
                }
                else
                {
                    UniversalAdditionalLightData lightdata = systemElement.ElemTransform.gameObject.GetComponent<UniversalAdditionalLightData>();
                    if (lightdata != null)
                    {
                        Destroy(lightdata);
                    }

                    Light light = systemElement.ElemTransform.gameObject.GetComponent<Light>();
                    if (light != null)
                    {
                        Destroy(light);
                    }


                    systemElement.ElemTransform.gameObject.GetComponent<Renderer>().material = defaultMat;
                }
            });

            isMoon.onValueChanged.AddListener((value) =>
            {
                
                if (value)
                {
                    if (systemElement.ElemTransform.gameObject.GetComponent<Light>() == null)
                    {
                        Light light = systemElement.ElemTransform.gameObject.AddComponent<Light>();
                        light.color = Color.white;
                        light.type = LightType.Point;
                        light.intensity = 5 * systemElement.Radius;
                        light.range = 10 * systemElement.Radius;
                        systemElement.ElemTransform.gameObject.GetComponent<Renderer>().material = moonMat;
                    }
                    else
                    {
                        isSun.isOn = false;
                    }
                }
                else
                {
                    UniversalAdditionalLightData lightdata = systemElement.ElemTransform.gameObject.GetComponent<UniversalAdditionalLightData>();
                    if (lightdata != null)
                    {
                        Destroy(lightdata);
                    }

                    Light light = systemElement.ElemTransform.gameObject.GetComponent<Light>();
                    if (light != null)
                    {
                        Destroy(light);
                    }


                    systemElement.ElemTransform.gameObject.GetComponent<Renderer>().material = defaultMat;
                }
            });

            // Switch the toggle to update the UI
            if (isSun.isOn)
            {
                isSun.isOn = false;
                isSun.isOn = true;
            }

            // Switch the toggle to update the UI
            if (isMoon.isOn)
            {
                isMoon.isOn = false;
                isMoon.isOn = true;
            }
        }

        private void Update()
        {
            // Check if clicked
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Input.mousePosition;
                RectTransform rectTransform = GetComponent<RectTransform>();
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePosition))
                {
                    OnClick?.Invoke(this);
                }
            }
        }

        public void Select(FillSystemElement fill)
        {
            if(fill.UI != null)
            {
                fill.LastUI = fill.UI;
            }

            fill.UI = this;

            
            fill.NameInput.text = systemElement.Name;
            fill.SpeedInput.text = systemElement.Speed.ToString();
            fill.RotationSpeedInput.text = systemElement.RotationSpeed.ToString();
            fill.RadiusInput.text = systemElement.Radius.ToString();

            if(fill.ColorPicker != null)
                fill.ColorPicker.color = systemElement.Color;

            if(systemElement.Orbit != null)
            {
                fill.OrbitToggle.isOn = systemElement.Orbit.ToggleOrbit;
                fill.OrbitRadius.text = systemElement.Orbit.Radius.ToString();
                fill.OrbitType.value = systemElement.Orbit is Circular ? 0 : 1;
                fill.OrbitDirection.value = (int)systemElement.Orbit.Direction;
            }
            else
            {
                fill.OrbitType.value = 0;
                this.systemElement.Orbit = null;
            }

            fill.ToggleOrbitUI(HasOrbit);
        }

    }
}