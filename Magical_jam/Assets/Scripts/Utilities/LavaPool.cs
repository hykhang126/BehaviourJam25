using Characters;
using UnityEngine;

namespace Utility
{
    public class LavaPool : Weapon
    {
        [SerializeField] private Collider2D lavaPoolCollider;
        [SerializeField] private float maxDamage;
        [SerializeField] private float destroyTime;

        private float spawnTime;
        private float currentDamage;
        private float damageDecrement;
        private bool isActive;

        private void Awake()
        {
            spawnTime = Time.time;
            currentDamage = maxDamage;
            damageDecrement = maxDamage / destroyTime;
            isActive = true;
        }

        private void Update()
        {
            if (!isActive)
            {
                gameObject.SetActive(false);
                return;
            }
            
            gameObject.SetActive(true);
            
            if (Time.time - spawnTime < destroyTime)
            {
                currentDamage -= damageDecrement * Time.deltaTime;
                return;
            }
            
            Destroy(gameObject);
        }

        public void SetActive(bool active)
        {
            isActive = active;
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