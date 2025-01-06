using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Ex3
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text text;

        public void Show(string message)
        {
            text.text = message;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
