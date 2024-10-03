using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _trolley, _cable;
    [SerializeField] private Child _near, _far;
    [SerializeField] private float _minHeight, _maxHeight;

    private GameObject _tower;

    private Child _trolleyT;

    private static PlayerController _instance;


    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        _tower = this.gameObject;
        _trolleyT = _trolley.GetComponent<Child>();
    }


    public void RotateTower(float angle)
    {
        // Rotate the tower around the Y axis
        _tower.transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void MoveTrolley(float distance)
    {
        // Lerp the position of the trolley using the near and far Children objects
        Vector3 nearPos = _near.RelativePosition;
        Vector3 farPos = _far.RelativePosition;

        _trolleyT.RelativePosition = Vector3.Lerp(nearPos, farPos, distance);
    }

    public void MoveCable(float distance)
    {
        // Lerp the scale of the cable between the min and max heights
        _cable.transform.localScale = Vector3.Lerp(new Vector3(1, _minHeight, 1), new Vector3(1, _maxHeight, 1), distance);
    }

    public Transform GetTrolleyTransform()
    {
        return _trolley.transform;
    }

    public float GetTrolleyPositionOnArm()
    {
        return Vector3.Distance(_near.transform.position, _trolley.transform.position) / Vector3.Distance(_near.transform.position, _far.transform.position);
    }

    public Vector3 GetTrolleyToTowerVector()
    {
        return _trolley.transform.position - _tower.transform.position;
    }

    public float RotationAngle
    {
        get
        {
            return _tower.transform.rotation.eulerAngles.y;
        }
    }

    public static PlayerController Instance
    {
        get
        {
            return _instance;
        }
    }
}
