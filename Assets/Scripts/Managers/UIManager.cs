using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager handling the UI, from HUD to menus
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Image loadingImg;
    [SerializeField] private Transform cursorAnchor;
    [SerializeField] private Toggle invertXToggle, invertYToggle;
    [SerializeField] private Slider masterSlider, musicSlider, soundsSlider;
    [SerializeField] private Slider sensitivityXSlider, sensitivityYSlider;

    //todo find a way to reduce the numbers of parameters in editor. maybe a settings manager ?
    
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
    /// Opens/closes the pause panel while adjusting timescale instantaneously
    /// </summary>
    /// <param name="isPausing">Wether we pause, or no</param>
    public void Pause(bool isPausing)
    {
        Time.timeScale = isPausing ? 0f : 1f;
        pausePanel.SetActive(isPausing);
        GameManager.Instance.LevelManager.Player.Pause(isPausing);
        pausePanel.GetComponentInChildren<Selectable>().Select();
    }

    /// <summary>
    /// Quitting the game, through the Game Manager
    /// </summary>
    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

    /// <summary>
    /// Updates the loading circle near the visor to the given percentage
    /// </summary>
    /// <param name="percent">The percent</param>
    public void LoadingFill(float percent)
    {
        loadingImg.fillAmount = percent;
    }

    /// <summary>
    /// Adjusts the cursor pointing towards the item held
    /// </summary>
    /// <param name="isVisible">Wether it is visible</param>
    /// <param name="angle">The angle, starting from top</param>
    public void AdjustCursor(bool isVisible, float angle)
    {
        cursorAnchor.gameObject.SetActive(isVisible);
        cursorAnchor.rotation = Quaternion.Euler(0f, 0f, angle);
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
    /// Changes wether the values of the mouse on the x-axis is inverted
    /// </summary>
    /// <param name="x">Wether it is inverted, or not</param>
    public void ChangeInvertX(bool x)
    {
        GameManager.Instance.LevelManager.Player.InvertAxisX = x;
    }
    
    /// <summary>
    /// Changes wether the values of the mouse on the y-axis is inverted
    /// </summary>
    /// <param name="y">Wether it is inverted, or not</param>
    public void ChangeInvertY(bool y)
    {
        GameManager.Instance.LevelManager.Player.InvertAxisY = y;
    }

    /// <summary>
    /// Refreshes the settings with the current ones
    /// </summary>
    public void RefreshSettings()
    {
        PlayerController p = GameManager.Instance.LevelManager.Player;
        invertXToggle.isOn = p.InvertAxisX;
        invertYToggle.isOn = p.InvertAxisY;
        masterSlider.value = GameManager.Instance.SoundManager.GetFloat("MasterVolume");
        musicSlider.value = GameManager.Instance.SoundManager.GetFloat("MusicVolume");
        soundsSlider.value = GameManager.Instance.SoundManager.GetFloat("SoundsVolume");
        sensitivityXSlider.value = p.MouseSensitivity.x;
        sensitivityYSlider.value = p.MouseSensitivity.y;
    }
}