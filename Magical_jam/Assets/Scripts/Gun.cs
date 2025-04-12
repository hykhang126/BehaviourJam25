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
        var trajectoryVector = mousePosition - transform.rotation.eulerAngles;
        var prefabAngle = Vector3.Angle(trajectoryVector, Vector3.right);
        var prefabRotation = Quaternion.Euler(0f, 0f, prefabAngle);
        
        var bullet = Instantiate(bulletPrefab, transform.position, prefabRotation);
        bullet.GetComponent<Bullet>().Initialize(trajectoryVector);
    }
}
