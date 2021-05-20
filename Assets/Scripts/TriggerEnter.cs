using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class calling UnityEvents when a collider enters in the trigger of this object
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerEnter : MonoBehaviour
{
    [Serializable] public class TriggerEnterEvent : UnityEvent<Transform> {}
    [SerializeField] private TriggerEnterEvent onInteract = new TriggerEnterEvent();
    [SerializeField] private string[] triggerTags = {"Player"};

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Any(t => other.transform.CompareTag(t)))
        {
            onInteract.Invoke(other.transform);
        }
    }
}