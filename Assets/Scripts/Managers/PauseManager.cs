using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private TextMeshProUGUI statisticsNbr;

    /// <summary>
    /// Opens/closes the pause panel while adjusting timescale instantaneously
    /// </summary>
    /// <param name="isPausing">Wether we pause, or not</param>
    public void Pause(bool isPausing)
    {
        Time.timeScale = isPausing ? 0f : 1f;
        pausePanel.SetActive(isPausing);
        GameManager.Instance.LevelManager.Player.Pause(isPausing);
        if (isPausing)
        {
            pausePanel.GetComponentInChildren<Selectable>().Select();
            StatisticsManager sm = GameManager.Instance.StatisticsManager;
            statisticsNbr.text = sm.Score + "\n" + sm.VasesPicked + "\n " + 
                                 Mathf.RoundToInt(sm.DistanceWalked) + "m\n " + Mathf.RoundToInt(sm.TimeSpent) + "s";
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