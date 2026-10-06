using UnityEngine;
using UnityEngine.UI;
using TMPro;                          // TextMeshPro, for the score text
using UnityEngine.SceneManagement;    // SceneManager.LoadScene and sceneLoaded
using System.Collections.Generic;     // List<T>

// GameManager
// The "brain" of the game. Tracks the score, the current level, and whether
// the game is playing, paused, or over. Decides what scene to load next.
//
// This uses the Singleton pattern: there is only ever one GameManager, and
// any script can reach it through GameManager.Instance without needing a
// reference set in the Inspector.
//
// It lives in Level 1 and uses DontDestroyOnLoad so it survives when
// Level 2, Level 3, and the in-between scenes load. When Level 1 is loaded
// again later, the extra copy in that scene removes itself in Awake.
//
// Important: the GameManager survives scene changes, but the objects it
// points to in a scene (like the Score text) do NOT. So every time a level
// loads, OnSceneLoaded finds that level's Score text again.
public class GameManager : MonoBehaviour
{

    // An enum is a list of named options. The game is always in exactly one of these.
    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }

    // The single shared GameManager. "private set" means other scripts can
    // read it but only this class can change it.
    public static GameManager Instance { get; private set; }

    // Other scripts (PlayerController, PlayerLook, ChasePlayer) check this before doing anything.
    public GameState CurrentState { get; private set; } = GameState.GameOver;

    // Coins collected in the current level
    public int score { get; private set; } = 0;


    // Scene names in play order. Must match the scene file names exactly,
    // and each scene must be in Build Settings to load.
    [SerializeField] private List<string> LevelNames = new List<string>();

    // Which song in SoundManager's music list to play during levels
    [SerializeField] private int musicIndex = 2;

    // Name of the text object each level uses to show the score.
    // Every level needs a UI text object with exactly this name.
    [SerializeField] private string scoreTextName = "Score";

    // Position in LevelNames. 0 = the first level.
    private int currentLevelIndex = 0;

    // Total coins in the current level. Each coin adds 1 to this in its
    // own Start (see Pickup.cs), so we never have to count them by hand.
    private int coinsToCollect = 0;

    // The score text in the CURRENT level. Found again each time a level loads.
    private TMP_Text scoreText;

    // Awake runs before Start. This is the standard Singleton setup.
    void Awake()
    {
        if (Instance == null)  // No GameManager yet, so this one becomes THE GameManager
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep this object alive when new scenes load
        }
        else  // A GameManager already exists (we came back to Level 1), so remove the duplicate
        {
            Destroy(gameObject);
        }
    }

    // sceneLoaded is an event: Unity calls every method "subscribed" to it
    // each time a scene finishes loading. += subscribes, -= unsubscribes.
    // Always unsubscribe in OnDisable, or a destroyed object can still get called.
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Runs after a scene loads, after every object in it has run Awake,
    // but BEFORE any of them run Start. That timing matters: we reset the
    // coin count here, and then each coin adds itself in its own Start.
    // This also runs for the very first scene when you press Play.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Instance != this)  // The duplicate copy in Level 1 also gets this call. Let the real one handle it.
        {
            return;
        }

        int index = LevelNames.IndexOf(scene.name);  // Where this scene is in LevelNames, or -1 if it isn't a level
        if (index < 0)  // Menus, Cut Scene, and Game Over scenes are not levels, so there is nothing to set up
        {
            return;
        }

        currentLevelIndex = index;  // Works even if you press Play from inside Level 2 or 3 for testing
        coinsToCollect = 0;

        // Find this level's score text by name. GameObject.Find returns null
        // if nothing has that name, so check before using it.
        GameObject scoreObject = GameObject.Find(scoreTextName);
        if (scoreObject != null)
        {
            scoreText = scoreObject.GetComponent<TMP_Text>();
        }
        else
        {
            scoreText = null;
            Debug.LogWarning($"No object named '{scoreTextName}' in {scene.name}, so the score will not show.");
        }

        StartPlay();
    }

    // Called by each coin's Start method to register itself
    public void AddCoinToCollect()
    {
        coinsToCollect++;
        UpdateScoreText();  // Keep the total on screen correct as coins register
    }

    // Called by a coin when the player picks it up
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        if (score >= coinsToCollect)  // If every coin in the level has been collected, the level is over
        {
            LevelOver();
        }
    }

    // Shows the score, e.g. "3 / 30". Skips the update if this level has no score text.
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{score} / {coinsToCollect}";
        }
    }

    // Resets the score and puts the game into the Playing state.
    // Called by OnSceneLoaded each time a level loads.
    public void StartPlay()
    {
        Time.timeScale = 1f;  // 1 = normal speed. Done first so nothing below can leave the game frozen.
        CurrentState = GameState.Playing;
        score = 0;
        UpdateScoreText();

        // SoundManager is created in the MainMenu scene. If you press Play
        // from inside a level it won't exist, so check before using it.
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StartMusic(musicIndex);
        }
    }

    // Time.timeScale = 0 freezes anything that uses Time.deltaTime and stops physics.
    // Update still runs, it just gets a deltaTime of 0.
    public void PauseGame()
    {
        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
    }

    // All coins collected. Either go to the cut scene or, on the last level, win.
    private void LevelOver()
    {
        PauseGame();

        if (currentLevelIndex == LevelNames.Count - 1)  // If the current level is the last level, the player has won
        {
            GameOver(false);
        }
        else
        {
            SceneManager.LoadScene("Cut Scene"); // Show the between-levels scene. Its Next Level button calls LoadNextLevel.
        }
    }

    // Called from the Cut Scene's Next Level button (through MainMenu.NextLevel).
    // StartPlay is not called here. LoadScene finishes at the end of the frame,
    // and OnSceneLoaded calls StartPlay once the new level actually exists.
    public void LoadNextLevel()
    {
        if (currentLevelIndex + 1 < LevelNames.Count)
        {
            SceneManager.LoadScene(LevelNames[currentLevelIndex + 1]); // Load the next level in the LevelNames list
        }
        else
        {
            GameOver(false); // If there are no more levels, trigger the GameOver state
        }
    }

    // Called from the "Retry" button on the GameOverCaught scene
    public void ReplayLevel()
    {
        SceneManager.LoadScene(LevelNames[currentLevelIndex]); // Reload the level the player was on
    }


    // Ends the game. "= false" makes the parameter optional, so
    // GameOver() and GameOver(false) mean the same thing.
    // The GameManager is NOT destroyed here. It needs to stay alive so the
    // Retry button knows which level to reload. Score and coins reset
    // automatically the next time a level loads.
    public void GameOver(bool wasCaught = false)
    {
        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;  // Freeze the level. Menu scenes use unscaled time, so their timers still run.

        if (wasCaught)
        {
            SceneManager.LoadScene("GameOverCaught"); // Load the "GameOverCaught" scene if the player was caught
        }
        else
        {
            SceneManager.LoadScene("GameOverWon"); // Load the "GameOverWon" scene if the player won
        }
    }

    // Toggles pause. Meant for a pause button (not hooked up in any scene right now).
    public void ButtonClicked()
    {
        if (CurrentState == GameState.Playing)
        {
            PauseGame();
        }
        else if (CurrentState == GameState.Paused)
        {
            ResumeGame();
        }
    }


}
