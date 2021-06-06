/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class calling UnityEvents when a collider exits the trigger of this object
 */

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class calling UnityEvents when a collider exits the trigger of this object
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerExit : MonoBehaviour
{
    [Serializable] public class TriggerExitEvent : UnityEvent<Transform> {}
    [SerializeField] private TriggerExitEvent onInteract = new TriggerExitEvent();
    [SerializeField] private string[] triggerTags = {"Player"};

    private void OnTriggerExit(Collider other)
    {
        if (triggerTags.Any(t => other.transform.CompareTag(t)))
        {
            onInteract.Invoke(other.transform);
        }
    }
}