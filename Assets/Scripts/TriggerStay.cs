using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//todo test

/// <summary>
/// Class calling UnityEvents when a collider stays in the trigger of this object for a certain time
/// </summary>
[RequireComponent(typeof(Collider))]
public class TriggerStay : MonoBehaviour
{
    [Serializable] public class TriggerStayEvent : UnityEvent<Transform> {}
    [SerializeField] private TriggerStayEvent onInteract = new TriggerStayEvent();
    [SerializeField] private string[] triggerTags = {"Player"};
    [SerializeField, Tooltip("in seconds, the time the tagged object has to stay within the trigger")] private float timeToStay = 1f;
    private Coroutine _waitCoroutine;

    private void OnTriggerExit(Collider other)
    {
        StopCoroutine(_waitCoroutine);
    }

    private IEnumerator Wait(Transform t)
    {
        yield return new WaitForSeconds(timeToStay);
        onInteract.Invoke(t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Any(t => other.transform.CompareTag(t)))
        {
            _waitCoroutine = StartCoroutine(Wait(other.transform));
        }
    }
}