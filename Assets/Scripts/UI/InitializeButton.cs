/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class ensuring the last button used can not be unselected
 */

using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class ensuring the last button used can not be unselected
/// </summary>
public class InitializeButton : MonoBehaviour
{
    private GameObject _lastSelect;

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            EventSystem.current.SetSelectedGameObject(_lastSelect);
        }
        else
        {
            _lastSelect = EventSystem.current.currentSelectedGameObject;
        }
    }
}