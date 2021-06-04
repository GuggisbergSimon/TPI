using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

/// <summary>
/// Class making an object interactable by the player
/// </summary>
public class ItemInteract : MonoBehaviour
{
    private enum ButtonType
    {
        OnOff,
        Repeat
    }

    [SerializeField] private ButtonType type = ButtonType.Repeat;
    [Serializable] public class InteractEvent : UnityEvent<Transform> {}
    [SerializeField] private InteractEvent onInteract = new InteractEvent();
    [SerializeField, Tooltip("If the type is OnOff, it will be called in Off mode.")]
    private UnityEvent onDisable = new UnityEvent();
    [SerializeField, Tooltip("Wether the button is on by default")]
    private bool isButtonOn = false;
    [SerializeField, Tooltip("A negative value means there is no limit.")]
    private int numberUses = -1;
    [SerializeField, Tooltip("The parent transform holding the object in its entirety")]
    private Transform parent;
    [SerializeField] private string helpText = "Use e to interact";
    [SerializeField] private AudioClip[] sounds;
    private AudioSource _audioSource;

    private bool CanUseButton => numberUses < 0 || numberUses > 0;

    private void Awake()
    {
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    /// <summary>
    /// Interacts with the object
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Interact()
    {
        switch (type)
        {
            case ButtonType.OnOff:
                isButtonOn = !isButtonOn;
                if (isButtonOn && CanUseButton)
                {
                    UseButton();
                }
                else if (CanUseButton)
                {
                    PlayOneSoundRandom();
                    UseDisable();
                }

                break;
            case ButtonType.Repeat:
                if (CanUseButton)
                {
                    UseDisable();
                    UseButton();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Displays the appropriate help text when called
    /// </summary>
    public void DisplayHelp()
    {
        GameManager.Instance.UiManager.HudManager.HelpInteract(true, helpText);
    }

    private void UseDisable()
    {
        onDisable.Invoke();
    }

    private void PlayOneSoundRandom()
    {
        _audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }

    private void UseButton()
    {
        PlayOneSoundRandom();
        onInteract.Invoke(parent);
        if (numberUses > 0)
        {
            numberUses--;
            if (numberUses <= 0)
            {
                Destroy(this);
            }
        }
    }
}