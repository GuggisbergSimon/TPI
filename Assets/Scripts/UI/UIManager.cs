/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Manager handling the UI, only holds links to more specific UI managers
 */

using UnityEngine;

/// <summary>
/// Manager handling the UI, only holds links to more specific UI managers
/// </summary>
public class UIManager : MonoBehaviour
{
    private HUDManager _hudManager;
    public HUDManager HudManager => _hudManager;
    private PauseManager _pauseManager;
    public PauseManager PauseManager => _pauseManager;

    private void Awake()
    {
        _hudManager = FindObjectOfType<HUDManager>();
        _pauseManager = FindObjectOfType<PauseManager>();
    }
}