using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class CableUI : MonoBehaviour
{
    private float _cableDistance;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }

    private void Update()
    {
        _cableDistance = _slider.value;
    }

    public void SendMove()
    {
        PlayerController.Instance.MoveCable(_cableDistance);
    }

}
