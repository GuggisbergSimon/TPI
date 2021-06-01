using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel, endPanel;
    [SerializeField] private TextMeshProUGUI[] statistics;

    /// <summary>
    /// Opens/closes the pause panel while adjusting timescale instantaneously, also freezes player
    /// </summary>
    /// <param name="isPausing">Wether we pause, or not</param>
    public void Pause(bool isPausing)
    {
        OpenUI(pausePanel, isPausing);
    }
    
    
    //todo test
    /// <summary>
    /// Opens the end panel, adjusting timescale and freezing player
    /// </summary>
    public void End()
    {
        OpenUI(endPanel, true);
        GameManager.Instance.LevelManager.Player.Pause(PlayerController.PlayerState.End);
    }

    private void OpenUI(GameObject g, bool isPausing)
    {
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
            statisticsNbr.text = sm.Score + "\n" + sm.VasesPicked + "\n " +
                                 Mathf.RoundToInt(sm.DistanceWalked) + "m\n " +
                                 Mathf.RoundToInt(sm.TimeSpent) + "s";
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
}