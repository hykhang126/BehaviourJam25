using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Levels;
using TMPro;
using Combat;
using System;
using Characters;

public class HUD : MonoBehaviour
{
    [SerializeField] Player player;

    /*
        Top left is green
        Top right is blue
        Bottom right is red
        Bottom left is yellow
    */
    [SerializeField] Image colorWheel;

    [SerializeField] GameObject gameOver;

    [SerializeField] Animator healthBarAnimator;
    [SerializeField] Animator healthHeartAnimator;

    [SerializeField] float _levelColorSwapCooldown = 0f;

    [SerializeField] LevelColorManager levelColorManager;

    // Make singleton
    public static HUD instance;
    private void Awake()
    {
        instance = this;

        // Set myself active
        gameObject.SetActive(true);
    }

    public LevelColor _currentColor;

    // SUBSCRIPTIONS TO EVENT OnLevelColorChanged
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentColor = newColor;
        UpdateColorWheel();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);
        
        if (levelColorManager == null)
        {
            Debug.LogError("LevelColorManager is not assigned in the inspector.");
            _levelColorSwapCooldown = 5f; // Default value if not assigned
        }
        else
        {
            _levelColorSwapCooldown = levelColorManager.levelColorSwapCooldown;
        }

        if (healthBarAnimator == null)
        {
            healthBarAnimator = GetComponentInChildren<Animator>();
        }

        if (healthHeartAnimator == null)
        {
            healthHeartAnimator = GetComponentInChildren<Animator>();
        }

        if (colorWheel == null)
        {
            colorWheel = GetComponentInChildren<Image>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the color wheel per frambased on _levelColorSwapCooldown, each color should finish 90 degrees in that amount of time
        if (_levelColorSwapCooldown > 0f)
        {
            float angle = 90f / _levelColorSwapCooldown * Time.deltaTime;
            RotateColorWheel(angle);
        }
    }

    public void GameOver(){
        gameOver.SetActive(true);
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        // Play different animation based on health level
        // Low health = 0.25f, Medium health = 0.5f, High health = 0.75f
        if (healthPercentage <= 0.25f)
        {
            healthBarAnimator.SetTrigger("LowHealth");
            healthHeartAnimator.SetTrigger("LowHealth");
        }
        else if (healthPercentage <= 0.5f)
        {
            healthBarAnimator.SetTrigger("MediumHealth");
            healthHeartAnimator.SetTrigger("MediumHealth");
        }
        else if (healthPercentage <= 0.75f)
        {
            healthBarAnimator.SetTrigger("HighHealth");
            healthHeartAnimator.SetTrigger("HighHealth");
        }
        else
        {
            healthBarAnimator.SetTrigger("HighHealth");
            healthHeartAnimator.SetTrigger("HighHealth");
        }
    }

    public void RotateColorWheel(float angle)
    {
        // Rotate the color wheel by the specified angle
        colorWheel.transform.Rotate(0, 0, angle);
    }

    private void UpdateColorWheel()
    {
        // Rotate the wheel 120 degrees based on the current color with a smooth transition
        // Top left is green, top right is blue, bottom is red
        Quaternion targetRotation = Quaternion.identity;

        switch (_currentColor)
        {
            case LevelColor.Green:
            targetRotation = Quaternion.Euler(0, 0, -45);
            break;
            case LevelColor.Blue:
            targetRotation = Quaternion.Euler(0, 0, 45);
            break;
            case LevelColor.Red:
            targetRotation = Quaternion.Euler(0, 0, 135);
            break;
        }

        // StartCoroutine(SmoothRotate(colorWheel.transform, targetRotation, 2.0f)); // 0.5 seconds for transition
        colorWheel.transform.rotation = targetRotation; // Set the final rotation immediately for now
    }

    private IEnumerator SmoothRotate(Transform target, Quaternion targetRotation, float duration)
    {
        Quaternion initialRotation = target.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            target.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.rotation = targetRotation; // Ensure final rotation is exact
    }

    public void SetHeartAnimationTrigger(string triggerName)
    {
        healthHeartAnimator.SetTrigger(triggerName);
    }
    public void SetHeartAnimationBool(string boolName, bool value)
    {
        healthHeartAnimator.SetBool(boolName, value);
    }
}
