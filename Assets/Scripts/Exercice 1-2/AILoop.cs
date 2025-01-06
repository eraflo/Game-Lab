using System.Collections;
using UnityEngine;

public class AILoop : MonoBehaviour
{
    [SerializeField] private GameObject _ground;

    private GameObject _concrete;
    private GameObject _hook;

    private bool _canLaunchSequence = true;

    /// <summary>
    /// The sphere collider of the concrete that the hook is going to grab
    /// </summary>
    private SphereCollider _concreteHookingPart;

    private void Start()
    {
        _hook = GameObject.FindObjectOfType<HookGrab>().gameObject;
    }

    private void Update()
    {
        Camera m_Camera = Camera.main;
        Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && Input.GetMouseButtonDown(0) && hit.collider.tag == "HookGrabbable" && _canLaunchSequence)
        {
            _concrete = hit.collider.gameObject;
            _concreteHookingPart = _concrete.GetComponent<SphereCollider>();
            _canLaunchSequence = false;
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

    /// <summary>
    /// Do the second step where the trolley moves to the concrete
    /// </summary>
    /// <returns></returns>
    private IEnumerator SecondStep() 
    {
        float distance;
        float lastDistance = Mathf.Infinity;
        float dot;

        // Get trolley position between near and far points
        float distanceTrolley = PlayerController.Instance.GetTrolleyPositionOnArm();
        Vector3 projectionConcrete;
        Vector3 projectionTrolley;
        Vector3 direction;

        float baseIncrValue = 0.01f;  // Base increment value
        float incrValue;

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

            if(distance > lastDistance)
            {
                Debug.Log("Stop to prevent overshooting");
                break;
            }
            lastDistance = distance;

            // Dot product between the direction vector and the trolley to tower vector
            dot = Vector3.Dot(direction.normalized, PlayerController.Instance.GetTrolleyToTowerVector());

            // Decrease the increment value as the distance decreases
            incrValue = baseIncrValue * Mathf.Clamp(distance / 2.0f, 0.005f, 1.0f);

            // Update the distance of the trolley
            distanceTrolley += incrValue * Mathf.Sign(dot);

            PlayerController.Instance.MoveTrolley(distanceTrolley);

            yield return null;
        }
        while (distance > 0.1f);

        StartCoroutine(ThirdStep());

    }

    /// <summary>
    /// Move the cable until the hook is close to the concrete hooking part
    /// </summary>
    /// <returns></returns>
    private IEnumerator ThirdStep()
    {
        Collider hookCollider = _hook.GetComponent<Collider>();

        // Vector between hook and concrete hooking part
        Vector3 hook_concrete_direction;
        Vector3 concrete_collider_pos;
        Vector3 hook_collider_pos;
        float distance;
        float dot;
        float lengthCable = PlayerController.Instance.GetCableHeight();

        do
        {
            // Get the position of the concrete hooking part collider
            concrete_collider_pos = _concreteHookingPart.bounds.center;

            // Get the position of the hook collider
            hook_collider_pos = hookCollider.bounds.center;

            // Calculate the vector between the hook and the concrete hooking part
            hook_concrete_direction = hook_collider_pos - concrete_collider_pos;

            // Get the magnitude of the vector
            distance = hook_concrete_direction.magnitude;

            // Get the dot product between the direction vector and the trolley position
            dot = Vector3.Dot(hook_concrete_direction, concrete_collider_pos);

            // Update the length of the cable
            lengthCable = lengthCable + 0.01f * (dot > 0 ? 1 : -1);

            PlayerController.Instance.MoveCable(lengthCable);

            yield return null;
        }
        while (distance > 0.5f);

        StartCoroutine(FourthStep());
    }

    /// <summary>
    /// Lift the concrete until it reaches the top of the tower
    /// </summary>
    /// <returns></returns>
    private IEnumerator FourthStep()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Move the cable until the concrete reaches the top of the tower
        while (!PlayerController.Instance.IsMinHeight())
        {
            PlayerController.Instance.MoveCable(Mathf.Clamp(PlayerController.Instance.GetCableHeight() - 0.1f, 0, 1));
            yield return null;
        }

        StartCoroutine(FifthStep());
    }

    /// <summary>
    /// Generate a random position for the concrete and move it to that position
    /// </summary>
    /// <returns></returns>
    private IEnumerator FifthStep()
    {
        yield return new WaitForSeconds(1f);

        Destroy(_concrete.GetComponent<Child>());

        // Get the ground width and length for the random position
        float groundWidth = _ground.GetComponent<Collider>().bounds.extents.x;
        float groundLength = _ground.GetComponent<Collider>().bounds.extents.z;

        Vector3 randomPos;
        Vector3 projectedRandomPos;
        Vector3 vectorToNearTrolley;
        Vector3 vectorToFarTrolley;

        // Get near and far trolley positions
        vectorToNearTrolley = PlayerController.Instance.NearVector();
        vectorToFarTrolley = PlayerController.Instance.FarVector();

        // Project the vectors on the ground
        vectorToNearTrolley = Vector3.ProjectOnPlane(vectorToNearTrolley, _ground.transform.up);
        vectorToFarTrolley = Vector3.ProjectOnPlane(vectorToFarTrolley, _ground.transform.up);

        // Calculate near and far distances from center
        float nearDistance = Vector3.Distance(vectorToNearTrolley, PlayerController.Instance.transform.position);
        float farDistance = Vector3.Distance(vectorToFarTrolley, PlayerController.Instance.transform.position);

        bool isValidPosition = false;

        do
        {
            // New random position generated
            randomPos = new Vector3(Random.Range(-groundWidth, groundWidth), Random.Range(10, 20), Random.Range(-groundLength, groundLength));

            // Projection on ground
            projectedRandomPos = Vector3.ProjectOnPlane(randomPos, _ground.transform.up);

            // Calculate the distance between random position and the center
            float distance = Vector3.Distance(projectedRandomPos, Vector3.zero);

            // Check if the random position is inside the circle
            if (distance >= nearDistance && distance <= farDistance)
            {
                isValidPosition = true;
            }


            yield return null;
        }
        while (!isValidPosition);

        // Move concrete to the new position
        _concrete.transform.position = randomPos;

        // Rotate the concrete to face the tower
        Vector3 direction = PlayerController.Instance.transform.position - _concrete.transform.position;
        direction = Vector3.ProjectOnPlane(direction, _ground.transform.up);

        // As the concrete forward is in the same orientation as the length of the concrete, we use the right vector to calculate the angle
        Vector3 rightConcrete = _concrete.transform.right;
        float angle = Vector3.SignedAngle(rightConcrete, direction, Vector3.up);

        _concrete.transform.Rotate(Vector3.up, angle);

        _canLaunchSequence = true;
    }

    /// <summary>
    /// Do the rotation of the tower for the first step 
    /// </summary>
    /// <param name="projection"></param>
    /// <returns></returns>
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
        float baseIncrValue = 1f;  // Base increment value
        float incrValue;

        while (angle <= -0.5f || angle >= 0.5f)
        {
            // Rotate the tower
            PlayerController.Instance.RotateTower(rotation);

            // Update the rotation
            incrValue = baseIncrValue * Mathf.Clamp(Mathf.Abs(angle) / 90.0f, 0.01f, 1.0f);
            rotation += incrValue * Mathf.Sign(angle);

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