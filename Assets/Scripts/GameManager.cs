using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        Playing,
        Paused,
        GameOver
    }
    public static GameManager Instance { get; private set; }

    public GameState CurrentState { get; private set; } = GameState.GameOver;

    public int score { get; private set; } = 0;


    [SerializeField] private List<string> LevelNames = new List<string>();

    [SerializeField] private int musicIndex = 2;

    private int currentLevelIndex = 0;

    private int coinsToCollect = 0;

    [SerializeField] private TMP_Text scoreText; // Reference to the TextMeshProUGUI component for displaying the score

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        StartPlay(); // Start the game in the Playing state when the game starts
    }

    public void AddCoinToCollect()
    {
        coinsToCollect++;
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = $"{score} / {coinsToCollect}"; // Initialize the score text at the start of the game
        if (score >= coinsToCollect)  // If the score exceeds the number of coins to collect, trigger the GameOver state
        {
            LevelOver();
        }
    }

    public void StartPlay()
    {
        SoundManager.Instance.StartMusic(musicIndex);
        CurrentState = GameState.Playing;
        score = 0;
        scoreText.text = $"{score} / {coinsToCollect}"; // Initialize the score text at the start of the game
        Time.timeScale = 1f;
    }

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

    private void LevelOver()
    {
        coinsToCollect = 0;

        PauseGame();

        if (currentLevelIndex == LevelNames.Count - 1)  // If the current level is the last level, trigger the GameOver state
        {
            GameOver(false);
        }
        else
        {
            SceneManager.LoadScene("Cut Scene"); // Load the next level in the LevelNames list
        }
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex < LevelNames.Count)
        {
            SceneManager.LoadScene(LevelNames[currentLevelIndex]); // Load the next level in the LevelNames list
            StartPlay(); // Start the game in the Playing state when the next level is loaded
        }
        else
        {
            GameOver(false); // If there are no more levels, trigger the GameOver state
        }
    }

    public void ReplayLevel()
    {

        SceneManager.LoadScene(LevelNames[currentLevelIndex]); // Load the next level in the LevelNames list
        StartPlay(); // Start the game in the Playing state when the next level is loaded
    }


    public void GameOver(bool wasCaught = false)
    {
        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;

        if (wasCaught)
        {
            SceneManager.LoadScene("GameOverCaught"); // Load the "GameOverCaught" scene if the player was caught
        }
        else
        {
            SceneManager.LoadScene("GameOverWon"); // Load the "GameOverWon" scene if the player won
        }

        Destroy(gameObject); // Destroy the GameManager instance when the game is over to reset the game state for the next playthrough
    }

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
