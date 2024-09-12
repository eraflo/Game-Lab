using GameMath.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerUI : MonoBehaviour
{
    [SerializeField] private HoldableButton _rotateLeft, _rotateRight;

    private float _angle;

    private void Start()
    {
        _angle = PlayerController.Instance.RotationAngle;
    }

    private void Update()
    {
        if (_rotateLeft.IsHeldDown)
        {
            RotateTowerLeft();
        }
        else if (_rotateRight.IsHeldDown)
        {
            RotateTowerRight();
        }
    }

    private void RotateTowerLeft()
    {
        _angle -= 1;
        PlayerController.Instance.RotateTower(_angle);
    }

    private void RotateTowerRight()
    {
        _angle += 1;
        PlayerController.Instance.RotateTower(_angle);
    }
}
