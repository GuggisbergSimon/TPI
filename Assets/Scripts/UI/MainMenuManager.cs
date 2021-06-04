using UnityEngine;

/// <summary>
/// Class used by the main menu to interface with the rest of the scripts
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Loads the main level, through the Game Manager
    /// </summary>
    public void LoadLevel()
    {
        GameManager.Instance.LoadLevel("MainScene");
    }

    /// <summary>
    /// Quits the game, through the Game Manager
    /// </summary>
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}