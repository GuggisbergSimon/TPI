using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manager handling the UI, from HUD to menus
/// </summary>
public class UIManager : MonoBehaviour
{
    /*
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Image loadingImg;
    [SerializeField] private Transform cursorAnchor;
    */
    [SerializeField] private Toggle invertXToggle, invertYToggle;
    [SerializeField] private Slider masterSlider, musicSlider, soundsSlider;
    [SerializeField] private Slider sensitivityXSlider, sensitivityYSlider;

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