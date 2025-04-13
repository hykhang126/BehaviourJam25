using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    private AudioSource audioSource;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayGame()
    {
        audioSource = GetComponent<AudioSource>();
        // audioSource.Stop();
        SceneManager.LoadSceneAsync("AmandaScene");
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
