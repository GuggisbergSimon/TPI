/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Main manager and static singleton, available at any time from any place in the code
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main manager and static singleton, available at any time from any place in the code
/// </summary>
public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    public UIManager UiManager => _uiManager;
    private LevelManager _levelManager;
    public LevelManager LevelManager => _levelManager;
    private StatisticsManager _statisticsManager;
    public StatisticsManager StatisticsManager => _statisticsManager;
    public static GameManager Instance { get; private set; }
    private Coroutine _timeScaleCoroutine;

    #region Unity Methods

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoadingScene;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoadingScene;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        Setup();
    }

    #endregion

    #region Custom Public Methods

    /// <summary>
    /// Hard reloads the current level
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Loads a given level
    /// </summary>
    /// <param name="nameLevel">The name of the level to load</param>
    public void LoadLevel(string nameLevel)
    {
        SceneManager.LoadScene(nameLevel);
    }

    /// <summary>
    /// Instantaneously quits the game
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    /// <summary>
    /// Changes time scale to the given scale in given seconds
    /// </summary>
    /// <param name="newTimeScale">new time to scale to</param>
    /// <param name="timeToChange">time to change to the new scale</param>
    public void ChangeTimeScale(float newTimeScale, float timeToChange)
    {
        if (_timeScaleCoroutine != null)
        {
            StopCoroutine(_timeScaleCoroutine);
        }

        timeToChange = timeToChange <= 0f ? StaticsValues.SMALLEST_POSITIVE_FLOAT : timeToChange;
        _timeScaleCoroutine = StartCoroutine(ChangingTimeScale(Time.timeScale, newTimeScale, 1 / timeToChange));
    }

    #endregion

    #region Custom Private Methods

    /// <summary>
    /// Coroutine gradually changing timescale
    /// </summary>
    /// <param name="a">initial timescale</param>
    /// <param name="b">final timescale</param>
    /// <param name="speed">the speed it changes</param>
    /// <returns>the time elapsed between each call of that coroutin</returns>
    private IEnumerator ChangingTimeScale(float a, float b, float speed)
    {
        for (float t = 0; t < 1f; t += Time.deltaTime * speed)
        {
            Time.timeScale = Mathf.Lerp(a, b, t);
            yield return null;
        }
    }

    //this function is called every time a scene is loaded, ensuring each manager known by the game manager is loaded up properly
    private void OnLevelFinishedLoadingScene(Scene scene, LoadSceneMode mode)
    {
        Setup();
    }

    /// <summary>
    /// Setup the GameManager for each scene, loading up different managers
    /// </summary>
    private void Setup()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        _statisticsManager = FindObjectOfType<StatisticsManager>();
    }

    #endregion
}