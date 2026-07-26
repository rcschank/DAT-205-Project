using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void LoadGame()
    {
        SceneManager.LoadScene("Level 1"); // Load the scene named "Level1" when the button is clicked
    }

    public void LoadTitle()
    {
        SceneManager.LoadScene("MainMenu"); // Load the scene named "Title" when the button is clicked
    }

    public void NextLevel()
    {
        GameManager.Instance.LoadNextLevel(); // Call the LoadNextLevel method in the GameManager to load the next level
    }

}
