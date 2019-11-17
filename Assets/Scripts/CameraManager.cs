using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    public bool useFixedUpdate = false;
    public float followSpeed = 2;

    private Camera mainCamera;

    [SerializeField]
    private Transform target; //Target transform to follow.

    public static CameraManager instance;

    /// <summary>
    /// Assigns the instance of the class when the gameobject is enabled.
    /// </summary>
    private void OnEnable()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    /// <summary>
    /// deassigns the instance if the gameobject is disabled.
    /// </summary>
    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    /// <summary>
    /// Gets the main camera component.
    /// </summary>
    private void Start()
    {
        if (!mainCamera)
        {
            mainCamera = GetComponent<Camera>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useFixedUpdate)
            return;
        FollowTarget();
    }

    /// <summary>
    /// Called after a fixed amount of time.
    /// </summary>
    private void FixedUpdate()
    {
        if (!useFixedUpdate)
            return;
        FollowTarget();
    }

    /// <summary>
    /// sets the target of the camera.
    /// </summary>
    /// <param name="target"></param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// Follow the target.
    /// </summary>
    private void FollowTarget()
    {
        if (!target)
            return;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, transform.position.y, target.position.z), Time.deltaTime * followSpeed);
    }
}
