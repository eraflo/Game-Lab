using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ex3
{
    public class StarSystem : MonoBehaviour
    {
        [SerializeField]
        private List<BaseSystemElement> systemElements = new List<BaseSystemElement>();

        public List<BaseSystemElement> Elements { get => systemElements; }

        public bool IsWorking { get; set; } = false;

        private void Awake()
        {
            foreach (ISystemElement element in systemElements)
            {
                element.OnSystemCollide += OnSystemCollide;

                if (element.Orbit == null) continue;
                element.Orbit.ToggleOrbit = true;
            }

            IsWorking = true;
        }

        private void Update()
        {
            if (!IsWorking) return;

            foreach (ISystemElement element in systemElements)
            {
                if(element.Type == SystemElementType.Moon)
                {
                    // remove all planet having the moon as revolved planet
                    systemElements.RemoveAll(e => e.RevolvedPlanet == element);
                }

                if (element.RevolvedPlanet == null || element.Orbit == null)
                {
                    element.Rotate();
                    continue;
                }

                element.Rotate();
                element.Revolve();
            }
        }

        public void AddSystemElem(ISystemElement element)
        {
            systemElements.Add((BaseSystemElement)element);
            element.OnSystemCollide += OnSystemCollide;
        }

        public void RemoveSystemElem(ISystemElement element)
        {
            systemElements.Remove((BaseSystemElement)element);
            element.OnSystemCollide -= OnSystemCollide;
        }

        public bool HasStar()
        {
            bool hasStar = false;
            foreach (BaseSystemElement element in systemElements)
            {
                if (element.Type == SystemElementType.Star)
                {
                    hasStar = true;
                    break;
                }
            }
            return hasStar;
        }

        private void OnSystemCollide(ISystemElement element)
        {
            if(Elements.Contains((BaseSystemElement)element))
                IsWorking = false;
        }
    }
}