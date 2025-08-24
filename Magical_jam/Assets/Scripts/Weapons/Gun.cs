using Characters;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class Gun : MonoBehaviour
{
    [Header("Gun Setup")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float reloadCooldown = 0.5f;
    [SerializeField] private float reloadTime;

    private Animator animator;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the gun object.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the gun object.");
        }
    }

    public void Shoot(Vector3 mousePosition, string bulletOwner)
    {
        // Gun cooldown
        if (Time.time < reloadTime)
        {
            return;
        }
        reloadTime = Time.time + reloadCooldown;

        var trajectoryVector = mousePosition - bulletSpawnPoint.transform.position;
        trajectoryVector.z = 0f;
        // if the vector maginitude is too small, magnify it
        if (trajectoryVector.magnitude < 1.0f)
        {
            trajectoryVector *= 10f;
        }

        trajectoryVector.Normalize();

        Quaternion prefabRotation = Quaternion.Euler(0, 0,
                                Mathf.Atan2(trajectoryVector.y, trajectoryVector.x) * Mathf.Rad2Deg);

        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, prefabRotation);
        bullet.GetComponent<Bullet>().Initialize(trajectoryVector, bulletOwner);
    }
    
    
    public void ToggleGun(bool toggle)
    {
        if (toggle)
        {
            EnableGun();
        }
        else
        {
            DisableGun();
        }
    }

    public void DisableGun()
    {
        if (spriteRenderer == null)
            return;
        spriteRenderer.enabled = false; // Disable the sprite renderer
    }

    public void EnableGun()
    {
        if (spriteRenderer == null)
            return;
        spriteRenderer.enabled = true; // Enable the sprite renderer
    }

    public void SetTriggerAnimation(string triggerName)
    {
        if (animator == null)
            return;
        animator.SetTrigger(triggerName); // Trigger the animation
    }

    public void SetFloatAnimation(string floatName, float value)
    {
        if (animator == null)
            return;
        animator.SetFloat(floatName, value); // Set the float parameter for the animation
    }
}
