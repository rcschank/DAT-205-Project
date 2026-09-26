using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string sceneToLoad; // Reference to the scene to load when the button is clicked
    [SerializeField] private float delayBeforeAutoLoad = 2f; // Delay before loading the scene, in seconds. 

    [SerializeField] private UnityEngine.UI.Image countdownImage; // Reference to the Image component for displaying the countdown timer
    [SerializeField] private Sprite[] countdownSprites; // Array of sprites for the countdown timer (4, 3, 2, 1)

    [SerializeField] private AudioClip countdownSound;
    [SerializeField] private AudioClip startSound;

    [SerializeField] private UnityEngine.UI.Button startButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;

    [SerializeField] private int musicIndex = 0;

    
    private float countdownTimer = 0f; // Timer to track the elapsed time since the game started

    private int currentNumber = 5;

    public void StartGame()
    {
        startButton.image.enabled = false;
        quitButton.image.enabled = false;
        startButton.enabled = false;
        quitButton.enabled = false;

        countdownTimer = delayBeforeAutoLoad = 5f;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }


    [SerializeField] private bool showCountdown = false;    
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad); // Load the scene when the button is clicked
    }

    public void ReloadScene()
    {
        GameManager.Instance.ReplayLevel();
    }
    
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Load Scene");
    }
    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel(); // Call the LoadNextLevel method in the GameManager to load the next level
    }

    void Start()
    {
        SoundManager.Instance.StartMusic(musicIndex);
        countdownTimer = delayBeforeAutoLoad; // Initialize the timer to the delay value at the start of the game
    }

    void Update()
    {

        if (delayBeforeAutoLoad <= 0f) // If the delay is set to 0 or less, do not auto-load the scene
        {
            return;
        }

        countdownTimer -= Time.deltaTime;

        if (showCountdown) // If showCountdown is true and there is less than 4 seconds remaining, display the countdown timer in the console
        {
        if (countdownTimer > 4f)
        {
            countdownImage.enabled = false; // Disable the countdown image before the countdown starts
        }
        else if (countdownTimer > 3f)
        {
            if (currentNumber != 4)
            {
                currentNumber = 4;
                SoundManager.Instance.PlaySFC(countdownSound); // Play the countdown sound when the countdown starts
            }
            countdownImage.enabled = true; // Enable the countdown image when the countdown starts
            countdownImage.sprite = countdownSprites[0]; // 4
        }
        else if (countdownTimer > 2f)
        {
            if (currentNumber != 3)
            {
                currentNumber = 3;
                 SoundManager.Instance.PlaySFC(countdownSound); // Play the countdown sound when the countdown starts
            }
            countdownImage.sprite = countdownSprites[1]; // 3
        }
        else if (countdownTimer > 1f)
        {
            if (currentNumber != 2)
            {
                currentNumber = 2;
                 SoundManager.Instance.PlaySFC(countdownSound); // Play the countdown sound when the countdown starts
            }
            countdownImage.sprite = countdownSprites[2]; // 2
        }
        else if (countdownTimer > 0f)
        {
            if (currentNumber != 1)
            {
                currentNumber = 1;
                 SoundManager.Instance.PlaySFC(countdownSound); // Play the countdown sound when the countdown starts
            }
            countdownImage.sprite = countdownSprites[3]; // 1
        }
        else
            {
                 SoundManager.Instance.PlaySFC(startSound); // Play the countdown sound when the countdown starts
            }
        }


        if (countdownTimer <= 0f) // If the timer has reached 0, load the specified scene
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
