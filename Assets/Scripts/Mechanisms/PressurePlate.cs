/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class behaving like a pressure plate found in numerous games
 */

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

/// <summary>
/// Class behaving like a pressure plate found in numerous games
/// </summary>
[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour
{
    [Serializable] public class TriggerEvent : UnityEvent<Transform> {}
    [SerializeField] private TriggerEvent onEnter = new TriggerEvent();
    [SerializeField] private TriggerEvent onStay = new TriggerEvent();
    [SerializeField] private TriggerEvent onExit = new TriggerEvent();
    [SerializeField] private float timeToStay = 1f;
    [SerializeField] private string[] triggerTags = {"Player", "Item"};
    [SerializeField] private bool needConstantPressure = true;
    [SerializeField] private AudioClip[] sounds;
    private int _nbrItemsStaying;
    private Coroutine _waitCoroutine;
    private Animator _animator;
    private AudioSource _audioSource;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponentInChildren<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Any(t => other.transform.CompareTag(t)))
        {
            if (_nbrItemsStaying <= 0)
            {
                onEnter.Invoke(other.transform);
                _animator.SetTrigger("PressureDown");
                PlayOneSoundRandom();
                _waitCoroutine = StartCoroutine(Wait(other.transform));
            }

            _nbrItemsStaying++;
        }
    }
    
    private IEnumerator Wait(Transform t)
    {
        yield return new WaitForSeconds(timeToStay);
        onStay.Invoke(t);
        if (!needConstantPressure)
        {
            gameObject.SetActive(false);
        }
    }

    private void PlayOneSoundRandom()
    {
        _audioSource.PlayOneShot(sounds[Random.Range(0, sounds.Length)]);
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerTags.Any(t => other.transform.CompareTag(t)) && _waitCoroutine != null)
        {
            _nbrItemsStaying--;
            if (needConstantPressure && _nbrItemsStaying <= 0)
            {
                _animator.SetTrigger("PressureUp");
                PlayOneSoundRandom();
                StopCoroutine(_waitCoroutine);
                onExit.Invoke(other.transform);
            }
        }
    }
}