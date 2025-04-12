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
    
    public void Shoot(Vector3 mousePosition)
    {
        var trajectoryVector = mousePosition - bulletSpawnPoint.transform.position;
        trajectoryVector.Normalize();
        trajectoryVector.z = 0f;

        Quaternion prefabRotation = Quaternion.Euler( 0, 0, 
                                Mathf.Atan2 ( trajectoryVector.y, trajectoryVector.x ) * Mathf.Rad2Deg );
        
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, prefabRotation);
        bullet.GetComponent<Bullet>().Initialize(trajectoryVector);
    }
}
