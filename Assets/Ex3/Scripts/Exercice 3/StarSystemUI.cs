
using System;
using System.Collections;
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
        public GameObject PopUp;

        private int uiCount = 5;
        private List<SystemElementUI> elements = new List<SystemElementUI>();

        private SystemElementUI selected;
        private GameObject popUp;

        public List<SystemElementUI> Elements { get => elements; }

        private void Start()
        {
            // PopUp
            popUp = Instantiate(PopUp, this.transform.parent.transform);
            popUp.SetActive(false);

            if (starSystem == null)
            {
                Debug.LogError("Star System is not set.");
                return;
            }

            elements = new List<SystemElementUI>();
            uiCount = starSystem.Elements.Count;

            // Add UI for each element already in the system
            for (int i = 0; i < uiCount; i++)
            {
                GameObject ui = Instantiate(uiPrefab, UIHolder.transform);
                SystemElementUI systemElementUI = ui.GetComponent<SystemElementUI>();
                systemElementUI.systemElement = starSystem.Elements[i];
                elements.Add(systemElementUI);

                ISystemElement element = starSystem.Elements[i];
                element.OnSystemCollide += OnSystemCollide;
            }

            // Add Star if no star in the system
            if (starSystem.Elements.Count == 0)
            {
                AddStar();
            }

            if (elements.Count > 0)
                SelectElement(elements[0]);

            // Select Element when click on it
            foreach (SystemElementUI element in elements)
            {
                element.OnClick += SelectElement;
            }
        }

        

        public void StartSystem()
        {
            starSystem.IsWorking = true;
        }

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
            // If no star in the system, add a star
            if (starSystem.HasStar() == false)
            {
                AddStar();
            }


            BaseSystemElement element = Instantiate(systemElementPrefab, new Vector3(0, -1, 0), Quaternion.identity).GetComponent<BaseSystemElement>();
            element.Type = SystemElementType.Planet;
            element.Name = "Planet " + (starSystem.Elements.FindAll(e => e.Type == SystemElementType.Planet).Count);
            element.RevolvedPlanet = starSystem.Elements.FindLast(e => e.Type == SystemElementType.Star);
            starSystem.AddSystemElem(element);

            // Pop Up
            ShowPopUpAndDisappear("Planet Added to the Star '" + element.RevolvedPlanet.Name + "'", 2);


            element.OnSystemCollide += OnSystemCollide;

            GameObject ui = Instantiate(uiPrefab, UIHolder.transform);
            SystemElementUI systemElementUI = ui.GetComponent<SystemElementUI>();
            systemElementUI.systemElement = element;
            elements.Add(systemElementUI);

            systemElementUI.OnClick += SelectElement;
            LayoutRebuilder.ForceRebuildLayoutImmediate(UIHolder.GetComponent<RectTransform>());
        }

        public void RemoveElement()
        {
            if (elements.Count == 0)
                return;

            SystemElementUI systemElementUI = selected;

            int index = starSystem.Elements.IndexOf((BaseSystemElement)selected.systemElement);

            GameObject obj = starSystem.Elements[index].gameObject;
            ISystemElement element = starSystem.Elements[index];

            element.OnSystemCollide -= OnSystemCollide;

            systemElementUI.OnClick -= SelectElement;

            elements.Remove(systemElementUI);
            starSystem.RemoveSystemElem(element);
            Destroy(systemElementUI.gameObject);
            Destroy(obj);

            LayoutRebuilder.ForceRebuildLayoutImmediate(UIHolder.GetComponent<RectTransform>());
        }

        public void ShowPopUp(string message)
        {
            popUp.SetActive(true);
            popUp.GetComponent<PopUp>().Show(message);
        }

        public void ShowPopUpAndDisappear(string message, float time)
        {
            StartCoroutine(ShowPopUpAndDisappearAsync(message, time, popUp));
        }

        private void AddStar()
        {
            BaseSystemElement element = Instantiate(systemElementPrefab, new Vector3(0, -1, 0), Quaternion.identity).GetComponent<BaseSystemElement>();
            element.Type = SystemElementType.Star;
            element.Name = "Star " + (starSystem.Elements.FindAll(e => e.Type == SystemElementType.Star).Count);
            starSystem.AddSystemElem(element);

            ShowPopUpAndDisappear("Star Added Because No Star in the System", 2);

            element.OnSystemCollide += OnSystemCollide;

            GameObject ui = Instantiate(uiPrefab, UIHolder.transform);
            SystemElementUI systemElementUI = ui.GetComponent<SystemElementUI>();
            systemElementUI.systemElement = element;
            elements.Add(systemElementUI);

            systemElementUI.IsSun = true;

            systemElementUI.OnClick += SelectElement;
            LayoutRebuilder.ForceRebuildLayoutImmediate(UIHolder.GetComponent<RectTransform>());
        }

        private IEnumerator ShowPopUpAndDisappearAsync(string message, float time, GameObject PopUp = null)
        {
            PopUp.SetActive(true);
            PopUp.GetComponent<PopUp>().Show(message);

            yield return new WaitForSeconds(time);
            PopUp.SetActive(false);
        }

        private void OnSystemCollide(ISystemElement element)
        {
            // Show PopUp
            ShowPopUp("System Collide. Please Start Again After Changing Parameters.");
        }
    }
}