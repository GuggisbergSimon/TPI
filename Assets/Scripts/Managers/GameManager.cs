using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Main manager and static singleton, available at any time from any place in the code
/// </summary>
public class GameManager : MonoBehaviour
{
    private UIManager _uiManager;
    public UIManager UIManager => _uiManager;
    private LevelManager _levelManager;
    public LevelManager LevelManager => _levelManager;
    private SoundManager _soundManager;
    public SoundManager SoundManager => _soundManager;
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

        timeToChange = timeToChange <= 0f ? StaticsValues.SMALLEST_INT : timeToChange;
        _timeScaleCoroutine = StartCoroutine(ChangingTimeScale(Time.timeScale, newTimeScale, 1 / timeToChange));
    }

    #endregion

    #region Custom Private Methods

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

    private void Setup()
    {
        _uiManager = FindObjectOfType<UIManager>();
        _levelManager = FindObjectOfType<LevelManager>();
        _soundManager = FindObjectOfType<SoundManager>();
    }

    #endregion
}