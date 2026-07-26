using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [SerializeField] private TMP_Text scoreText; // Reference to the TextMeshProUGUI component for displaying the score
    [SerializeField] private TextMeshProUGUI buttonText; // Reference to the TextMeshProUGUI component for displaying the button text

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
        CurrentState = GameState.GameOver;
        score = 0;
        scoreText.text = score.ToString(); // Initialize the score text at the start of the game
        buttonText.text = "Play!"; // Initialize the button text at the start of the game
    }


    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString(); // Initialize the score text at the start of the game
        if (score > 25)  // If the score exceeds 25, trigger the GameOver state
        {
            GameOver();
        }
    }

    public void StartPlay()
    {
        CurrentState = GameState.Playing;
        score = 0;
        Time.timeScale = 1f;
        buttonText.text = "Pause"; // Change the button text to "Pause" when the game starts
    }

    public void PauseGame()
    {
        CurrentState = GameState.Paused;
        Time.timeScale = 0f;
        buttonText.text = "Resume"; // Change the button text to "Resume" when the game is paused
    }

    public void ResumeGame()
    {
        CurrentState = GameState.Playing;
        Time.timeScale = 1f;
        buttonText.text = "Pause"; // Change the button text to "Pause" when the game is resumed
    }

    public void GameOver()
    {
        CurrentState = GameState.GameOver;
        Time.timeScale = 0f;
        buttonText.text = "Play Again?"; // Change the button text to "Play Again?" when the game is over
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
        else if (CurrentState == GameState.GameOver)
        {
            StartPlay();
        }
    }


}
