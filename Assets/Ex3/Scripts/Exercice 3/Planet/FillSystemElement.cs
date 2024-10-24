using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ex3
{
    public class FillSystemElement : MonoBehaviour
    {
        public SystemElementUI UI { get; set; }

        [SerializeField]
        private StarSystemUI starSystemUI;

        [Header("Planet Properties")]
        [SerializeField]
        private TMP_InputField nameInput;
        [SerializeField]
        private TMP_InputField speedInput;
        [SerializeField]
        private TMP_InputField rotationSpeedInput;
        [SerializeField]
        private TMP_InputField radiusInput;
        [SerializeField]
        private TMP_InputField distanceInput;
        [SerializeField]
        private GameObject colorPicker;
        [SerializeField]
        private Button selectParent;

        [Header("Orbit Properties")]
        [SerializeField]
        private TMP_Dropdown orbitType;
        [SerializeField]
        private Toggle orbitToggle;

        private FlexibleColorPicker colorPickerScript;

        public TMP_InputField NameInput { get => nameInput; set => nameInput = value; }
        public TMP_InputField SpeedInput { get => speedInput; set => speedInput = value; }
        public TMP_InputField RotationSpeedInput { get => rotationSpeedInput; set => rotationSpeedInput = value; }
        public TMP_InputField RadiusInput { get => radiusInput; set => radiusInput = value; }
        public TMP_InputField DistanceInput { get => distanceInput; set => distanceInput = value; }
        public Button SelectParent { get => selectParent; set => selectParent = value; }

        public FlexibleColorPicker ColorPicker { get => colorPickerScript; set => colorPickerScript = value; }

        public TMP_Dropdown OrbitType { get => orbitType; set => orbitType = value; }
        public Toggle OrbitToggle { get => orbitToggle; set => orbitToggle = value; }


        private void Start()
        {
            colorPickerScript = colorPicker.GetComponent<FlexibleColorPicker>();

            // Initialize UI values
            nameInput.text = UI.systemElement.Name;
            speedInput.text = UI.systemElement.Speed.ToString();
            rotationSpeedInput.text = UI.systemElement.RotationSpeed.ToString();
            radiusInput.text = UI.systemElement.Radius.ToString();
            distanceInput.text = UI.systemElement.Distance.ToString();
            colorPickerScript.color = UI.systemElement.Color;

            nameInput.onValueChanged.AddListener((value) =>
            {
                UI.systemElement.Name = value;
                UI.NameText = value;
            });

            speedInput.onValueChanged.AddListener((value) =>
            {
                if (float.TryParse(value, out float speed))
                {
                    UI.systemElement.Speed = speed;
                    UI.SpeedText = "Speed : " + speed.ToString();
                }
            });

            rotationSpeedInput.onValueChanged.AddListener((value) =>
            {
                if (float.TryParse(value, out float rotationSpeed))
                {
                    UI.systemElement.RotationSpeed = rotationSpeed;
                    UI.RotationSpeedText = "Rotation Speed : " + rotationSpeed.ToString();
                }
            });

            radiusInput.onValueChanged.AddListener((value) =>
            {
                if (float.TryParse(value, out float radius))
                {
                    UI.systemElement.Radius = radius;
                    UI.RadiusText = "Radius : " + radius.ToString();
                }
            });

            distanceInput.onValueChanged.AddListener((value) =>
            {
                if (float.TryParse(value, out float distance))
                {
                    UI.systemElement.Distance = distance;
                    UI.DistanceText = "Distance : " + distance.ToString();
                }
            });

            colorPickerScript.onColorChange.AddListener((color) =>
            {
                UI.systemElement.Color = color;
            });

            orbitType.onValueChanged.AddListener((value) =>
            {
                switch (value)
                {
                    case 0:
                        UI.systemElement.Orbit = new Circular();
                        break;
                    case 1:
                        UI.systemElement.Orbit = new Elliptical();
                        break;
                }
            });

            orbitToggle.onValueChanged.AddListener((value) =>
            {
                UI.systemElement.Orbit.ToggleOrbit = value;
            });

            selectParent.onClick.AddListener(
                () =>
                {
                    foreach (SystemElementUI element in starSystemUI.Elements)
                    {
                        if(element != UI)
                            element.OnClick += OnSelectParent;
                    }
                }
            );
        }

        private void OnSelectParent(SystemElementUI element)
        {
            UI.systemElement.RevolvedPlanet = element.systemElement;

            foreach (SystemElementUI elementUI in starSystemUI.Elements)
            {
                elementUI.OnClick -= OnSelectParent;
            }
        }
    }
}