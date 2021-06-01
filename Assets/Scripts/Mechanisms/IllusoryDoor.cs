using System;
using UnityEngine;

public class IllusoryDoor : MonoBehaviour
{
    [SerializeField] private float distToCheck = 10f;
    [SerializeField, Range(0f, 90f)] private float angleDiffMax = 90f;
    [SerializeField] private bool enableSpriteBehaviour = false;
    private Collider[] _colliders;
    private SpriteRenderer[] _sprites;
    private bool _isOpen;

    private void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();
        _sprites = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        Transform p = GameManager.Instance.LevelManager.Player.transform;
        bool isCloseEnough = Vector3.Distance(p.position, transform.position) < distToCheck;
        if (_isOpen && !isCloseEnough)
        {
            Open(false);
        }

        if (!isCloseEnough) return;

        bool b = Vector3.Dot(p.forward, Vector3.ProjectOnPlane(
            p.position - transform.position, transform.up
        )) > Mathf.Cos(angleDiffMax);
        Open(b);
    }

    private void Open(bool value)
    {
        _isOpen = value;
        foreach (var collider1 in _colliders)
        {
            collider1.isTrigger = value;
        }

        if (enableSpriteBehaviour)
        {
            foreach (var s in _sprites)
            {
                s.gameObject.SetActive(!value);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position + Vector3.up * 0.5f, Vector3.one);
    }
}