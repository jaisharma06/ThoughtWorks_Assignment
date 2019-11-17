using UnityEngine;
using UnityEngine.UI;

public class Tank : MonoBehaviour
{
    [SerializeField]
    private TankType type; //Type of the tank i.e. HEAVY or LIGHT
    public float speed = 1; //Movement speed of the tank

    private int health = 2; //Health of the tank.
    private Rigidbody rb; //Rigidbody component of the tank.
    public bool isDead = false; //True if the tank is already destroyed.

    public Slider healthBar; //Health slider.
    public Image healthImage; //Health slider image.

    public bool isActive = false; //true, if tank can be interacted.

    //Boundary values of the tank.
    int minX;
    int maxX;
    int minY;
    int maxY;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Handles the movement of the tank on the basis of axis input.
    /// </summary>
    private void Update()
    {
        if (!isActive || isDead)
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
                Vector3 transformPos = transform.position;
                if (transformPos.x < minX)
                {
                    transformPos.x = minX;
                    transform.position = transformPos;
                }else if (transformPos.x > maxX)
                {
                    transformPos.x = maxX;
                    transform.position = transformPos;
                }

                if (transformPos.z < minY)
                {
                    transformPos.z = minY;
                    transform.position = transformPos;
                }
                else if (transformPos.z > maxY)
                {
                    transformPos.z = maxY;
                    transform.position = transformPos;
                }
            }
        }
        else
        {
            transform.position = new Vector3(Mathf.RoundToInt(transform.position.x), transform.position.y, Mathf.RoundToInt(transform.position.z));
        }
    }

    /// <summary>
    /// Set the boundaries of the tank. Beyond those tank cant move.
    /// </summary>
    /// <param name="minX">minimum X value of the tank position</param>
    /// <param name="maxX">maximum X value of the tank position</param>
    /// <param name="minY">minimum Z value of the tank position</param>
    /// <param name="maxY">maximum Z value of the tank position</param>
    public void SetBounds(int minX, int maxX, int minY, int maxY)
    {
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
    }

    /// <summary>
    /// Called to reduce the health of the tank.
    /// </summary>
    public void TakeHit()
    {
        health--;
        healthBar.value = health * 50;

        if (health <= 50)
        {
            healthImage.color = Color.yellow;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Destroys the tank.
    /// </summary>
    private void Die()
    {
        isDead = true;
        SetActive(true);
    }

    /// <summary>
    /// Called when the mouse is clicked on the tank.
    /// </summary>
    private void OnMouseDown()
    {
        EventManager.TankTouched(this);
    }

    /// <summary>
    /// Activates the tank for movement.
    /// </summary>
    public void Activate()
    {
        isActive = true;
        rb.isKinematic = false;
    }

    /// <summary>
    /// Deactivates the tank for movement.
    /// </summary>
    public void Deactivate()
    {
        isActive = false;
        rb.isKinematic = true;
    }

    /// <summary>
    /// Sets active and inactive.
    /// </summary>
    /// <param name="isActive">active if true.</param>
    public void SetActive(bool isActive)
    {
        if (!isDead)
            gameObject.SetActive(isActive);
        else
            gameObject.SetActive(true);
    }
}
