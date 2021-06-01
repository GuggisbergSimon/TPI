using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour
{
    [Serializable] public class TriggerEvent : UnityEvent<Transform> {}
    [SerializeField] private TriggerEvent onStay = new TriggerEvent();
    [SerializeField] private TriggerEvent onExit = new TriggerEvent();
    [SerializeField] private float timeToStay = 1f;
    [SerializeField] private string[] triggerTags = {"Player", "Item"};
    [SerializeField] private bool needConstantPressure = true;
    private int _nbrItemsStaying;
    private Coroutine _waitCoroutine;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (triggerTags.Any(t => other.transform.CompareTag(t)))
        {
            if (_nbrItemsStaying <= 0)
            {
                _animator.SetTrigger("PressureDown");
                _waitCoroutine = StartCoroutine(Wait(other.transform));
            }

            _nbrItemsStaying++;
        }
    }

    private IEnumerator Wait(Transform t)
    {
        yield return new WaitForSeconds(timeToStay);
        onStay.Invoke(t);
    }


    private void OnTriggerExit(Collider other)
    {
        if (triggerTags.Any(t => other.transform.CompareTag(t)) && _waitCoroutine != null)
        {
            _nbrItemsStaying--;
            if (needConstantPressure && _nbrItemsStaying <= 0)
            {
                _animator.SetTrigger("PressureUp");
                StopCoroutine(_waitCoroutine);
                onExit.Invoke(other.transform);
            }
        }
    }
}