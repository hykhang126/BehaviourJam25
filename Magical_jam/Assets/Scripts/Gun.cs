using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;

    [SerializeField] Player player;

    [SerializeField] GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(){
        GameObject bullet = Instantiate(bulletPrefab,spawnPoint.transform.position,transform.rotation);
    }
}
