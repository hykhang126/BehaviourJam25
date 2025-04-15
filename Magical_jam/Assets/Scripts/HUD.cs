using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Levels;
using TMPro;
using Combat;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Image colorWheel;

    [SerializeField] GameObject gameOver;

    [SerializeField] Animator healthBarAnimator;
    [SerializeField] Animator healthHeartAnimator;

    // Make singleton
    public static HUD instance;
    private void Awake()
    {
        instance = this;
    }

    public LevelColor _currentColor;

    // SUBSCRIPTIONS TO EVENT OnColorChange
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentColor = newColor;
        UpdateColorWheel();
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);

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

    private void UpdateColorWheel()
    {
        // Rotate the wheel 90 degrees based on the current color with a smooth transition
        Quaternion targetRotation = Quaternion.identity;

        switch (_currentColor)
        {
            case LevelColor.Green:
                targetRotation = Quaternion.Euler(0, 0, 0);
                break;
            case LevelColor.Blue:
                targetRotation = Quaternion.Euler(0, 0, 90);
                break;
            case LevelColor.Red:
                targetRotation = Quaternion.Euler(0, 0, 180);
                break;
            case LevelColor.Yellow:
                targetRotation = Quaternion.Euler(0, 0, 270);
                break;
        }

        StartCoroutine(SmoothRotate(colorWheel.transform, targetRotation, 2.0f)); // 0.5 seconds for transition
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
