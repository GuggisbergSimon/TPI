/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class used by the pause menu to display statistics and change settings
 */

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class used by the pause menu to display statistics and change settings
/// </summary>
public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel, endPanel, settingsPanel, helpPanel;
    [SerializeField] private TextMeshProUGUI[] statistics;
    [SerializeField] private Slider sensitivityXSlider, sensitivityYSlider;

    /// <summary>
    /// Opens/closes the pause panel while adjusting timescale instantaneously, also freezes player
    /// </summary>
    /// <param name="isPausing">Wether we pause, or not</param>
    public void Pause(bool isPausing)
    {
        OpenUI(pausePanel, isPausing);
    }

    /// <summary>
    /// Opens the end panel, adjusting timescale and freezing player
    /// </summary>
    public void End(bool isEnding)
    {
        OpenUI(endPanel, isEnding);
        Time.timeScale = 1f;
        GameManager.Instance.LevelManager.Player.Pause(PlayerController.PlayerState.End);
    }

    /// <summary>
    /// Opens a hook to the dev's itch page
    /// </summary>
    public void OpenHook()
    {
        Application.OpenURL("https://ataor.itch.io/");
    }

    /// <summary>
    /// Toggles a given UI
    /// </summary>
    /// <param name="ui">the UI to toggle</param>
    /// <param name="isPausing">Wether we are pausing or not</param>
    private void OpenUI(GameObject ui, bool isPausing)
    {
        Cursor.visible = isPausing;
        Cursor.lockState = isPausing ? CursorLockMode.None : CursorLockMode.Locked;
        settingsPanel.SetActive(false);
        helpPanel.SetActive(false);
        Time.timeScale = isPausing ? 0f : 1f;
        ui.SetActive(isPausing);
        GameManager.Instance.LevelManager.Player.Pause(isPausing
            ? PlayerController.PlayerState.Pause
            : PlayerController.PlayerState.Idle);
        if (!isPausing) return;
        EventSystem.current.SetSelectedGameObject(null);
        ui.GetComponentInChildren<Selectable>().Select();
        StatisticsManager sm = GameManager.Instance.StatisticsManager;
        foreach (var statisticsNbr in statistics)
        {
            float seconds = Mathf.RoundToInt(sm.TimeSpent);
            statisticsNbr.text = sm.GetScore() + "\n" + sm.VasesPicked +
                                 "/" + GameManager.Instance.LevelManager.NbrVases + "\n" +
                                 sm.NbrJumps + "\n" +
                                 Mathf.RoundToInt(sm.DistanceWalked) + "m\n" +
                                 Mathf.Floor(seconds / 60f) + "m" + seconds % 60f + "s\n";
        }
    }

    /// <summary>
    /// Loads the main menu, through the Game Manager
    /// </summary>
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.LoadLevel("MainMenu");
    }

    /// <summary>
    /// Quitting the game, through the Game Manager
    /// </summary>
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    /// <summary>
    /// Changes the sensitivity of the mouse on the x axis to the value
    /// </summary>
    /// <param name="x">The x-axis sensitivity</param>
    public void ChangeSensitivityMouseX(float x)
    {
        GameManager.Instance.LevelManager.Player.ChangeSensitivityX(x);
    }

    /// <summary>
    /// Changes the sensitivity of the mouse on the yaxis to the value
    /// </summary>
    /// <param name="y">The y-axis sensitivity</param>
    public void ChangeSensitivityMouseY(float y)
    {
        GameManager.Instance.LevelManager.Player.ChangeSensitivityY(y);
    }

    /// <summary>
    /// Refreshes the settings with the current ones
    /// </summary>
    public void RefreshSettings()
    {
        PlayerController p = GameManager.Instance.LevelManager.Player;
        sensitivityXSlider.value = p.MouseSensitivity.x;
        sensitivityYSlider.value = p.MouseSensitivity.y;
    }
}