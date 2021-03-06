/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class used by the main menu to interface with the rest of the scripts
 */

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
    
    /// <summary>
    /// Opens a hook to the dev's itch page
    /// </summary>
    public void OpenHook()
    {
        Application.OpenURL("https://ataor.itch.io/");
    }
}