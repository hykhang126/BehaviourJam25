using UnityEngine;

public class PlayerWeaponAnimCon : MonoBehaviour
{
    Animator animator;

    SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
    }

    public void ToggleGun(bool toggle)
    {
        if (toggle)
        {
            EnableGun();
        }
        else
        {
            DisableGun();
        }
    }

    public void DisableGun()
    {
        if (spriteRenderer == null)
            return;
        spriteRenderer.enabled = false; // Disable the sprite renderer
    }

    public void EnableGun()
    {
        if (spriteRenderer == null)
            return;
        spriteRenderer.enabled = true; // Enable the sprite renderer
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
