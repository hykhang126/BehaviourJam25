using System;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float reloadCooldown = 5f;
    
    void Start()
    {
    }
    
    void Update()
    {
    }
    
    public void Shoot(Vector3 mousePosition, string bulletOwner)
    {
        var trajectoryVector = mousePosition - bulletSpawnPoint.transform.position;
        trajectoryVector.z = 0f;
        // if the vector maginitude is too small, magnify it
        if (trajectoryVector.magnitude < 1.0f)
        {
            trajectoryVector *= 10f;
        }

        trajectoryVector.Normalize();

        Quaternion prefabRotation = Quaternion.Euler( 0, 0, 
                                Mathf.Atan2 ( trajectoryVector.y, trajectoryVector.x ) * Mathf.Rad2Deg );
        
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, prefabRotation);
        bullet.GetComponent<Bullet>().Initialize(trajectoryVector, bulletOwner);
    }
}
