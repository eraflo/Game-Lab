using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class TrolleyUI : MonoBehaviour
{
    private float _trolleyDistance;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        _trolleyDistance = _slider.value;
    }


    public void SendMove()
    {
        PlayerController.Instance.MoveTrolley(_trolleyDistance);
    }
}
