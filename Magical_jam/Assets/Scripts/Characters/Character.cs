using UnityEngine;

namespace Characters
{
    public class Character : MonoBehaviour
    {
        [Header(nameof(Character))]
        [SerializeField] protected Collider2D characterCollider;
        [SerializeField] protected Rigidbody2D characterRigidbody;
        [SerializeField] protected SpriteRenderer characterSpriteRenderer;
        [SerializeField] protected float health;
        [SerializeField] protected float moveSpeed;
    }
}