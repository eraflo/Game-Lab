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

        private void Start()
        {
            foreach (ISystemElement element in systemElements)
            {
                if(element.Orbit == null) continue;
                element.Orbit.ToggleOrbit = true;
            }
        }

        private void Update()
        {
            foreach (ISystemElement element in systemElements)
            {
                if(element.RevolvedPlanet == null || element.Orbit == null)
                {
                    element.Rotate();
                    continue;
                }

                Debug.Log(element.Name);

                element.Rotate();
                element.Revolve();
            }
        }
    }
}