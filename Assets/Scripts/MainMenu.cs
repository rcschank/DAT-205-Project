using UnityEngine;
using UnityEngine.SceneManagement;

// MainMenu
// One script reused by every menu-style scene. Each scene uses a different
// mix of its features, set up in the Inspector:
//   - MainMenu scene (splash):  waits 5 seconds, then loads "Load Scene"
//   - Load Scene (title menu):  Start and Quit buttons, 4-3-2-1 countdown, then Level 1
//   - Cut Scene:                Next Level button
//   - GameOverCaught:           Retry and Main Menu buttons, auto-returns after 10 seconds
//   - GameOverWon:              button back to MainMenu, auto-returns after 10 seconds
//
// The public methods are meant to be called from UI Button OnClick events.
public class MainMenu : MonoBehaviour
{

    [SerializeField] private string sceneToLoad; // Name of the scene to load when the timer runs out or LoadScene is called
    [SerializeField] private float delayBeforeAutoLoad = 2f; // Delay before loading the scene, in seconds. 0 turns auto-load off.

    [SerializeField] private UnityEngine.UI.Image countdownImage; // Reference to the Image component for displaying the countdown timer
    [SerializeField] private Sprite[] countdownSprites; // Array of sprites for the countdown timer (4, 3, 2, 1)

    [SerializeField] private AudioClip countdownSound;  // Plays on each number
    [SerializeField] private AudioClip startSound;      // Plays when the countdown hits zero

    // Writing UnityEngine.UI.Button in full works the same as adding
    // "using UnityEngine.UI;" at the top and writing just Button.
    [SerializeField] private UnityEngine.UI.Button startButton;
    [SerializeField] private UnityEngine.UI.Button quitButton;

    [SerializeField] private int musicIndex = 0;  // Which song in SoundManager's list plays in this scene


    private float countdownTimer = 0f; // Seconds left before the scene auto-loads. Counts DOWN.

    // The number currently on screen. Used so each number's sound plays only
    // once instead of every frame. Starts at 5 so 4 counts as "new".
    private int currentNumber = 5;

    // Start button OnClick (Load Scene). Hides the buttons and starts the countdown.
    public void StartGame()
    {
        // Hide the button graphics and stop them from being clicked again
        startButton.image.enabled = false;
        quitButton.image.enabled = false;
        startButton.enabled = false;
        quitButton.enabled = false;

        // Sets BOTH variables. This turns auto-load on (it was 0 in this
        // scene) and starts the timer.
        countdownTimer = delayBeforeAutoLoad = countdownLength;
    }

    // The 4-3-2-1 countdown in Update is built around 5 seconds:
    // one blank second, then one second for each number.
    // const means this value can never change while the game runs.
    private const float countdownLength = 5f;

    // Plays a sound effect through SoundManager, if there is one.
    // SoundManager is created in the MainMenu scene, so it won't exist if
    // you press Play from another scene while testing.
    private void PlaySound(AudioClip clip)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySFC(clip);
        }
    }

    // Quit button OnClick.
    // #if is checked when the game is compiled: in the Editor we stop Play
    // mode, in a real build we close the application.
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }


    [SerializeField] private bool showCountdown = false;    // Turn on to show the 4-3-2-1 images (only used in Load Scene)
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad); // Load the scene when the button is clicked
    }

    // Retry button OnClick (GameOverCaught)
    public void ReloadScene()
    {
        GameManager.Instance.ReplayLevel();
    }

    // Main Menu button OnClick (GameOverCaught). "Load Scene" is the title menu.
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Load Scene");
    }
    // Next Level button OnClick (Cut Scene)
    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel(); // Call the LoadNextLevel method in the GameManager to load the next level
    }

    void Start()
    {
        if (SoundManager.Instance != null)  // Missing if you press Play from this scene instead of MainMenu
        {
            SoundManager.Instance.StartMusic(musicIndex);  // StartMusic does nothing if this song is already playing
        }
        countdownTimer = delayBeforeAutoLoad; // Initialize the timer to the delay value at the start of the game
    }

    void Update()
    {

        if (delayBeforeAutoLoad <= 0f) // If the delay is set to 0 or less, do not auto-load the scene
        {
            return;
        }

        // unscaledDeltaTime ignores Time.timeScale. The GameManager sets
        // timeScale to 0 when a level ends, and it stays 0 in these menu
        // scenes. With plain deltaTime (always 0 then) this timer would never count down.
        countdownTimer -= Time.unscaledDeltaTime;

        if (showCountdown) // If showCountdown is true, swap the countdown image based on how much time is left
        {
        // Each branch covers one second of the countdown.
        // The "if (currentNumber != X)" check makes the sound play only on
        // the first frame of each number.
        if (countdownTimer > 4f)
        {
            countdownImage.enabled = false; // Disable the countdown image before the countdown starts
        }
        else if (countdownTimer > 3f)
        {
            if (currentNumber != 4)
            {
                currentNumber = 4;
                PlaySound(countdownSound); // Play the countdown sound once when 4 appears
            }
            countdownImage.enabled = true; // Enable the countdown image when the countdown starts
            countdownImage.sprite = countdownSprites[0]; // 4
        }
        else if (countdownTimer > 2f)
        {
            if (currentNumber != 3)
            {
                currentNumber = 3;
                 PlaySound(countdownSound); // Play the countdown sound once when 3 appears
            }
            countdownImage.sprite = countdownSprites[1]; // 3
        }
        else if (countdownTimer > 1f)
        {
            if (currentNumber != 2)
            {
                currentNumber = 2;
                 PlaySound(countdownSound); // Play the countdown sound once when 2 appears
            }
            countdownImage.sprite = countdownSprites[2]; // 2
        }
        else if (countdownTimer > 0f)
        {
            if (currentNumber != 1)
            {
                currentNumber = 1;
                 PlaySound(countdownSound); // Play the countdown sound once when 1 appears
            }
            countdownImage.sprite = countdownSprites[3]; // 1
        }
        else
            {
                 PlaySound(startSound); // Time is up: play the start sound (SoundManager survives the scene change, so it keeps playing)
            }
        }


        if (countdownTimer <= 0f) // If the timer has reached 0, load the specified scene
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

}
