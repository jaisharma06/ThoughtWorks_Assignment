using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField]
    private TankType type;
    public float speed = 1;

    private int hitCount = 2;
    private Rigidbody rb;

    public bool isActive = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!isActive)
            return;
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput != 0 || horizontalInput != 0)
        {
            if (rb)
            {
                Vector3 velocity = new Vector3(horizontalInput * Time.deltaTime, 0, verticalInput * Time.deltaTime) * speed;
                if (verticalInput != 0)
                {
                    velocity.x = 0;
                }
                else
                {
                    velocity.z = 0;
                }
                
                rb.velocity = velocity;
                transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);
            }
        }
        else
        {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));
        }
    }

    public void TakeHit()
    {
        hitCount--;
        if (hitCount <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

    }
}
