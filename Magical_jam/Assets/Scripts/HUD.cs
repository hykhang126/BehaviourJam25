using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Levels;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Image barkIcon;

    [SerializeField] Image[] healthDisplay;

    [SerializeField] GameObject gameOver;

    public Slider healthSlider;

    public TextMeshProUGUI currentColorText;

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
    }

    public void GameOver(){
        barkIcon.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(true);
    }

    public void UpdateHealthBar(float healthPercentage)
    {
        // Fill the silder based on the health percentage
        healthSlider.value = healthPercentage;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);

        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
            return;
        }
        healthSlider.value = 1f; // Set the initial value of the health slider to 1 (full health)

        if (currentColorText == null)
        {
            currentColorText = GetComponentInChildren<TextMeshProUGUI>();
            return;
        }
        currentColorText.text = _currentColor.ToString(); // Set the current color text to the player's color

    }

    // Update is called once per frame
    void Update()
    {
        currentColorText.text = _currentColor.ToString(); // Update the current color text to the player's color
        // Change text color depoending on the current color
        switch (_currentColor)
        {
            case LevelColor.Red:
                currentColorText.color = Color.red;
                break;
            case LevelColor.Green:
                currentColorText.color = Color.green;
                break;
            case LevelColor.Blue:
                currentColorText.color = Color.blue;
                break;
            case LevelColor.Yellow:
                currentColorText.color = Color.yellow;
                break;
            default:
                currentColorText.color = Color.black; // Default color if no match
                break;
        }
    }
}
