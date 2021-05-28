using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class calling UnityEvents when a collider enters in the trigger of this object
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerExit : MonoBehaviour
{
    [Serializable] public class TriggerExitEvent : UnityEvent<Transform> {}
    [SerializeField] private TriggerExitEvent onInteract = new TriggerExitEvent();
    [SerializeField] private string[] triggerTags = {"Player"};

    private void OnTriggerExit(Collider other)
    {
        //todo check if triggerstay and handle it correctly
        if (triggerTags.Any(t => other.transform.CompareTag(t)))
        {
            onInteract.Invoke(other.transform);
        }
    }
}