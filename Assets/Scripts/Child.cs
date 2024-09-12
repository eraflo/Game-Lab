using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : MonoBehaviour
{
    [SerializeField] private Transform _parent;

    private Vector3 _relativePosition;
    private Vector3 _relativeAxisY, _relativeAxisZ;

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        // Make the child follow the parent
        transform.position = _parent.position + _parent.transform.TransformVector(_relativePosition);
        transform.rotation = Quaternion.LookRotation(_parent.transform.TransformDirection(_relativeAxisZ), _parent.transform.TransformDirection(_relativeAxisY));
    }

    public void Initialize()
    {
        _relativePosition = _parent.transform.InverseTransformVector(transform.position - _parent.position);
        _relativeAxisY = _parent.transform.InverseTransformDirection(transform.up);
        _relativeAxisZ = _parent.transform.InverseTransformDirection(transform.forward);
    }

    public Vector3 RelativePosition
    {
        get
        {
            return _relativePosition;
        }
        set
        {
            _relativePosition = value;
        }
    }

    public Transform Parent
    {
        get
        {
            return _parent;
        }
        set
        {
            _parent = value;
        }
    }
}
