using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 2;
    public GameObject explosion;
    Rigidbody rb;
    public AudioClip explosionClip;
    private AudioSource _as;

    /// <summary>
    /// Sets the audiosource component.
    /// </summary>
    private void Awake()
    {
        _as = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Damages the target tank.
    /// </summary>
    /// <param name="initPosition">Initial position of the bullet.</param>
    /// <param name="targetPosition">target position of the bullet</param>
    /// <param name="targetTank">target tank</param>
    public void DamageTank(Vector3 initPosition, Vector3 targetPosition, Tank targetTank)
    {
        SetActive(true);
        StartCoroutine(MoveToTarget(initPosition, targetPosition, targetTank));
    }

    /// <summary>
    /// Sets the bullet active or inactive.
    /// </summary>
    /// <param name="isActive">active if true.</param>
    private void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    /// <summary>
    /// Async movement towards the target
    /// </summary>
    /// <param name="initPosition"></param>
    /// <param name="targetPosition"></param>
    /// <param name="targetTank"></param>
    /// <returns></returns>
    IEnumerator MoveToTarget(Vector3 initPosition, Vector3 targetPosition, Tank targetTank)
    {
        transform.position = new Vector3(initPosition.x, transform.position.y, initPosition.z);
        transform.rotation = Quaternion.LookRotation(targetPosition - initPosition, Vector3.up);
        while (transform.position.x != targetPosition.x || transform.position.z != targetPosition.z)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), Time.deltaTime * moveSpeed);
            yield return null;
        }
        explosion.SetActive(true);
        _as.PlayOneShot(explosionClip);
        if (targetTank)
            targetTank.TakeHit();
        yield return new WaitForSeconds(0.2f);
        SetActive(false);
        explosion.SetActive(false);
        GameManager.instance.ToggleTurn();
    }
}
