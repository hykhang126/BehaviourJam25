using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    
    [SerializeField] EnemyBullet bulletPrefab;
    
    [SerializeField] private float fireRate;
    [SerializeField] private int bulletCount;
    [SerializeField] private Transform muzzle;
    private float nextFireTime;
    
    
    public void Shoot()
    {
        
        if (Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, muzzle.transform.position, transform.rotation);
            nextFireTime = Time.time + fireRate;
            
        }        
    }
}
