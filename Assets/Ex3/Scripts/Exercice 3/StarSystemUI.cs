
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ex3
{
    public class StarSystemUI : MonoBehaviour
    {
        public StarSystem starSystem;
        public GameObject systemElementPrefab;

        [Header("UI")]
        public FillSystemElement fillSystemElement;
        public GameObject UIHolder;
        public GameObject uiPrefab;

        private int uiCount = 5;
        private List<SystemElementUI> elements = new List<SystemElementUI>();

        private SystemElementUI selected;

        private void Start()
        {
            elements = new List<SystemElementUI>();
            uiCount = starSystem.Elements.Count;

            for (int i = 0; i < uiCount; i++)
            {
                GameObject ui = Instantiate(uiPrefab, UIHolder.transform);
                SystemElementUI systemElementUI = ui.GetComponent<SystemElementUI>();
                systemElementUI.systemElement = starSystem.Elements[i];
                elements.Add(systemElementUI);
            }

            if (elements.Count > 0)
                SelectElement(elements[0]);

            // Select Element when click on it
            foreach (SystemElementUI element in elements)
            {
                element.OnClick += SelectElement;
            }
        }

        public List<SystemElementUI> Elements { get => elements; }

        public void SelectElement(SystemElementUI element)
        {
            if (selected != null)
                selected.GetComponent<Image>().color = Color.white;

            selected = element;
            selected.GetComponent<Image>().color = Color.green;

            selected.Select(fillSystemElement);
        }

        public void AddElement()
        {
            BaseSystemElement element = Instantiate(systemElementPrefab, new Vector3(0, -1, 0), Quaternion.identity).GetComponent<BaseSystemElement>();
            starSystem.Elements.Add(element);

            GameObject ui = Instantiate(uiPrefab, UIHolder.transform);
            SystemElementUI systemElementUI = ui.GetComponent<SystemElementUI>();
            systemElementUI.systemElement = element;
            elements.Add(systemElementUI);

            systemElementUI.OnClick += SelectElement;
        }

        public void RemoveElement()
        {
            if (elements.Count == 0)
                return;

            SystemElementUI systemElementUI = selected;
            GameObject obj;

            int index = starSystem.Elements.IndexOf((BaseSystemElement)selected.systemElement);

            obj = starSystem.Elements[index].gameObject;

            systemElementUI.OnClick -= SelectElement;

            elements.Remove(systemElementUI);
            starSystem.Elements.RemoveAt(index);
            Destroy(systemElementUI.gameObject);
            Destroy(obj);
        }
    }
}