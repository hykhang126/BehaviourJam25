using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Image barkIcon;

    [SerializeField] Image[] healthDisplay;

    [SerializeField] GameObject gameOver;



    public void barked(){
        barkIcon.enabled = false;
    }

    public void canBark(){
        barkIcon.enabled=true;
    }

    public void lowerHealth(){
        healthDisplay[player.getHealth()].gameObject.SetActive(false);
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
