using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HookGrab : MonoBehaviour
{
    [SerializeField] private ParticleSystem _hookParticles;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HookGrabbable") && other is SphereCollider)
        {
            Child c = other.AddComponent<Child>();
            c.Parent = this.transform;
            c.Initialize();
            _hookParticles.transform.position = other.transform.position;
            _hookParticles.Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HookGrabbable") && other is SphereCollider)
        {
            Destroy(other.GetComponent<Child>());
        }
    }
}
