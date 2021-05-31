using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Loads the main level, through the Game Manager
    /// </summary>
    public void LoadLevel()
    {
        GameManager.Instance.LoadLevel("SampleScene");
    }

    /// <summary>
    /// Quitting the game, through the Game Manager
    /// </summary>
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
