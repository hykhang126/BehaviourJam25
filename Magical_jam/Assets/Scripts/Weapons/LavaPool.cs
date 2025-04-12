using System;
using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class LavaPool : MonoBehaviour
    {
        [SerializeField] private Collider2D lavaPoolCollider;
        [SerializeField] private float maxDamage;
        [SerializeField] private float destroyTime;

        private float spawnTime;
        private float currentDamage;
        private float damageDecrement;

        private void Awake()
        {
            spawnTime = Time.time;
            currentDamage = maxDamage;
            damageDecrement = maxDamage / destroyTime;
        }

        private void Update()
        {
            if (Time.time - spawnTime < destroyTime)
            {
                currentDamage -= damageDecrement * Time.deltaTime;
                return;
            }
            
            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.gameObject.GetComponent<Player>().TakeDamage(currentDamage);
            }
        }
    }
}