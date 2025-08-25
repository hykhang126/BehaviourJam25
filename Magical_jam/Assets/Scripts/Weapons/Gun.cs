using Characters;
using UnityEngine;

public class Gun : Weapon
{
    [Header("Gun Setup")]
    [SerializeField] private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float reloadCooldown = 0.5f;
    [SerializeField] private float reloadTime;

    protected override void Start()
    {
        base.Start();
    }

    public void Shoot(Vector3 mousePosition, string bulletOwner)
    {
        // Gun cooldown
        if (Time.time < reloadTime)
        {
            return;
        }
        // Reload
        reloadTime = Time.time + reloadCooldown;
        // Animation
        SetTriggerAnimation("Shoot");

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
            EnableSprite();
        }
        else
        {
            DisableSprite();
        }
    }
}
