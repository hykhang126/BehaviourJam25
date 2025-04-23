using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Levels;
using TMPro;
using Combat;
using System;
using Unity.VisualScripting;

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
    [SerializeField] TMP_Text colorText;

    [SerializeField] GameObject gameOver;

    [SerializeField] LevelColorManager levelColorManager;
    [SerializeField] float _levelColorSwapCooldown = 0f;

    [SerializeField] Animator healthBarAnimator;
    [SerializeField] Animator healthHeartAnimator;
    
    [Header("Health Blip")]
    HealthBlip healthBlip;
    [SerializeField] GameObject healthBlipPrefab;
    [SerializeField] GameObject healthBlipPrefabContainer;
    [SerializeField] Transform healthBlipEndPos;

    [Header("Level Color")]
    [SerializeField] LevelColor _currentColor;
    // SUBSCRIPTIONS TO EVENT OnColorChange
    public void UpdateCurrentColor(LevelColor newColor)
    {
        _currentColor = newColor;
        UpdateColorWheel();
    }

    // MAKE SINGLETON
    public static HUD instance;
    public void Awake()
    {
        instance = this;
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

        if (healthBlip == null)
        {
            healthBlip = healthBlipPrefab.GetComponent<HealthBlip>();
            healthBlip.frequencyTimer.onTimerEnd += OnBlipFrequencyEnd;
            healthBlip.endPosition = healthBlipEndPos;
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

        if (player.GetHealth() > 0f) SendBlipsToRight();
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
        // Rotate the wheel 90 degrees based on the current color with a smooth transition
        // Top left is green, top right is blue, bottom right is red, bottom left is yellow
        Quaternion targetRotation = Quaternion.identity;

        switch (_currentColor)
        {
            case LevelColor.Green:
            targetRotation = Quaternion.Euler(0, 0, -45);
            colorText.SetText("ENVY");
            break;
            case LevelColor.Blue:
            targetRotation = Quaternion.Euler(0, 0, 45);
            colorText.SetText("MELANCHOLY");
            break;
            case LevelColor.Red:
            targetRotation = Quaternion.Euler(0, 0, 135);
            colorText.SetText("RAGE");
            break;
            case LevelColor.Yellow:
            targetRotation = Quaternion.Euler(0, 0, 225);
            colorText.SetText("SHOCK");
            break;
        }

        colorWheel.transform.rotation = targetRotation; // Set the final rotation immediately for now
    }

    public void OnBlipFrequencyEnd()
    {
        var spawnLocation = healthBlipPrefabContainer.transform.position;
        // Spawn a new blip, also increase the size by 5 times
        var blip = Instantiate(healthBlipPrefab, spawnLocation, Quaternion.identity);
        blip.transform.localScale *= 5f; // Increase the size by 5 times
        blip.transform.SetParent(healthBlipPrefabContainer.transform, true);
    }

    public void SendBlipsToRight()
    {
        // Find blip frequency according to player health
        // 1 blip every 2 second at full health, decreasing with player health
        // min 0.5 second at 1 health
        float blipFrequency = 2f;
        healthBlip.frequencyBetweenBlips = blipFrequency * (player.GetHealth() / player.GetMaxHealth());
        healthBlip.frequencyBetweenBlips = Mathf.Clamp(
            healthBlip.frequencyBetweenBlips, 
            0.5f, 
            healthBlip.frequencyBetweenBlips);
        // Update the blip frequency timer
        if (healthBlip.frequencyTimer.IsRunning()) 
            healthBlip.frequencyTimer.UpdateTime(healthBlip.frequencyBetweenBlips);
        else
            healthBlip.frequencyTimer.SetTime(healthBlip.frequencyBetweenBlips);
        healthBlip.frequencyTimer.Tick(Time.deltaTime);
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
