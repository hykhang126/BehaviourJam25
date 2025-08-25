using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Weapon : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the shield object.");
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer component not found on the shield object.");
        }
        spriteRenderer.enabled = false; // Initially disable the sprite renderer
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

    public void SetBoolAnimation(string boolName, bool value)
    {
        if (animator == null)
            return;
        animator.SetBool(boolName, value); // Set the bool parameter for the animation
    }

    public void DisableSprite()
    {
        if (spriteRenderer == null)
            return;
        spriteRenderer.enabled = false; // Disable the sprite renderer
    }

    public void EnableSprite()
    {
        if (spriteRenderer == null)
            return;
        spriteRenderer.enabled = true; // Enable the sprite renderer
    }
}
