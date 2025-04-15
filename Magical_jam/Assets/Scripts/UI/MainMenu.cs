using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;
    private AudioSource audioSource;

    [SerializeField] private string firstSceneToPlay;

    // void Awake()
    // {
    //     if(instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    public void PlayGame()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        if (string.IsNullOrEmpty(firstSceneToPlay))
        {
            SceneManager.LoadScene("AmandaScene");
        }
        else SceneManager.LoadScene(firstSceneToPlay);
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
