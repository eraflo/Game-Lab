using System.Collections;
using UnityEngine;

public class AILoop : MonoBehaviour
{
    [SerializeField] private GameObject _ground;

    private GameObject _concrete;

    private void Update()
    {
        Camera m_Camera = Camera.main;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && Input.GetMouseButtonDown(0) && hit.collider.tag == "HookGrabbable")
        {
            _concrete = hit.collider.gameObject;
            FirstStep();
        }
    }



    /// <summary>
    /// Crane Starts to rotate around world Y axis and stop when its arm facing Concrete in top down view
    /// </summary>
    private void FirstStep()
    {
        // Player Controller on the tower
        Vector3 towerPos = PlayerController.Instance.transform.position;

        // Concrete position
        Vector3 concretePos = _concrete.transform.position;

        // Vector from tower to concrete
        Vector3 direction = concretePos - towerPos;

        // Projection on ground
        Vector3 projection = Vector3.ProjectOnPlane(direction, _ground.transform.up);

        StartCoroutine(RotateFirstStep(projection));
    }

    private IEnumerator SecondStep() 
    {
        float distance;
        float dot;

        // Get trolley position between near and far points
        float distanceTrolley = PlayerController.Instance.GetTrolleyPositionOnArm();
        Vector3 projectionConcrete;
        Vector3 projectionTrolley;
        Vector3 direction;

        do
        {
            // Projection concrete on ground
            projectionConcrete = Vector3.ProjectOnPlane(_concrete.transform.position, _ground.transform.up);

            // Projection trolley on ground
            projectionTrolley = Vector3.ProjectOnPlane(PlayerController.Instance.GetTrolleyTransform().position, _ground.transform.up);

            // Vector between the trolley and the concrete points created by the projections
            direction = projectionConcrete - projectionTrolley;

            // Get magnitude of the vector
            distance = direction.magnitude;

            // Dot product between the trolley and the concrete
            dot = Vector3.Dot(projectionConcrete, projectionTrolley);

            distanceTrolley = distanceTrolley + 0.01f * (dot > 0 ? 1 : -1);

            PlayerController.Instance.MoveTrolley(distanceTrolley);

            Debug.Log("Distance: " + distance);
            Debug.Log("Trolley: " + distanceTrolley);
            Debug.Log("Dot: " + dot);

            yield return null;
        }
        while (distance > 0.1f);

    }

    private IEnumerator RotateFirstStep(Vector3 projection)
    {
        // Get the vector from the trolley to the tower
        Vector3 arm = PlayerController.Instance.GetTrolleyToTowerVector();

        // Project the arm vector on the ground
        arm = Vector3.ProjectOnPlane(arm, _ground.transform.up);

        // Get the angle between the arm and the projection
        float angle = Vector3.SignedAngle(arm, projection, Vector3.up);

        // Setup
        float rotation = PlayerController.Instance.RotationAngle;

        while (angle <= -1f || angle >= 1f)
        {
            // Rotate the tower
            PlayerController.Instance.RotateTower(rotation);

            // Update the rotation
            rotation += (angle > 0 ? 0.1f : -0.1f);

            // Update the vector from the trolley to the tower
            arm = PlayerController.Instance.GetTrolleyToTowerVector();
            arm = Vector3.ProjectOnPlane(arm, _ground.transform.up);

            // Update the angle
            angle = Vector3.SignedAngle(arm, projection, Vector3.up);

            yield return null;
        }

        StartCoroutine(SecondStep());
    }

    private void OnDrawGizmos()
    {

    }
}