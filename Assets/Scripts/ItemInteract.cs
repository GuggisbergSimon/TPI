using System;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField] private UnityEvent onInteract = new UnityEvent();
    [SerializeField, Tooltip("If the type is OnOff, it will be called in Off mode.")] private UnityEvent onDisable = new UnityEvent();
    [SerializeField, Tooltip("Wether the button is on by default")] private bool isButtonOn = false;
    [SerializeField, Tooltip("A negative value means there is no limit.")] private int numberUses = -1;

    private bool CanUseButton => numberUses < 0 || numberUses > 0;

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

    private void UseDisable()
    {
        onDisable.Invoke();
    }

    private void UseButton()
    {
        onInteract.Invoke();
        if (numberUses > 0)
        {
            numberUses--;
        }
    }
}