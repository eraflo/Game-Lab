

using TMPro;
using UnityEngine;
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
        [SerializeField]
        private TMP_Text distanceText;


        public ISystemElement systemElement { get; set; }

        public string NameText { get => nameText.text; set => nameText.text = value; }
        public string SpeedText { get => speedText.text; set => speedText.text = value; }
        public string RotationSpeedText { get => rotationSpeedText.text; set => rotationSpeedText.text = value; }
        public string RadiusText { get => radiusText.text; set => radiusText.text = value; }
        public string DistanceText { get => distanceText.text; set => distanceText.text = value; }

        private void Start()
        {
            NameText = systemElement.Name;
            SpeedText = "Speed : " + systemElement.Speed.ToString();
            RotationSpeedText = "Rotation Speed : " + systemElement.RotationSpeed.ToString();
            RadiusText = "Radius : " + systemElement.Radius.ToString();
            DistanceText = "Distance : " + systemElement.Distance.ToString();
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
            fill.UI = this;
        }

    }
}