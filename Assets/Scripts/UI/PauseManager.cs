using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel, endPanel, settingsPanel;
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
    public void End()
    {
        GameManager.Instance.LevelManager.Player.Pause(PlayerController.PlayerState.End);
        OpenUI(endPanel, true);
        Time.timeScale = 1f;
    }

    private void OpenUI(GameObject g, bool isPausing)
    {
        settingsPanel.SetActive(false);
        Time.timeScale = isPausing ? 0f : 1f;
        g.SetActive(isPausing);
        GameManager.Instance.LevelManager.Player.Pause(isPausing
            ? PlayerController.PlayerState.Pause
            : PlayerController.PlayerState.Idle);
        if (!isPausing) return;
        g.GetComponentInChildren<Selectable>().Select();
        StatisticsManager sm = GameManager.Instance.StatisticsManager;
        foreach (var statisticsNbr in statistics)
        {
            statisticsNbr.text = sm.GetScore() + "\n" + sm.VasesPicked + "\n" +
                                 sm.NbrJumps + "\n" +
                                 Mathf.RoundToInt(sm.TimeSpent) + "s\n" +
                                 Mathf.RoundToInt(sm.DistanceWalked) + "m\n";
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
    /// Changes the master volume
    /// </summary>
    /// <param name="volume">The new volume</param>
    public void ChangeVolumeMaster(float volume)
    {
        GameManager.Instance.SoundManager.SetFloat("MasterVolume", volume);
    }

    /// <summary>
    /// Changes the music volume
    /// </summary>
    /// <param name="volume">The new volume</param>
    public void ChangeVolumeMusic(float volume)
    {
        GameManager.Instance.SoundManager.SetFloat("MusicVolume", volume);
    }

    /// <summary>
    /// Changes the sounds volume
    /// </summary>
    /// <param name="volume">The given volume</param>
    public void ChangeVolumeSounds(float volume)
    {
        GameManager.Instance.SoundManager.SetFloat("SoundsVolume", volume);
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