using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Levels;

public class HUD : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Image barkIcon;

    [SerializeField] Image[] healthDisplay;

    [SerializeField] GameObject gameOver;

    public Slider weaponTempSlider;

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

    
    // Start is called before the first frame update
    void Start()
    {
        gameOver.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
