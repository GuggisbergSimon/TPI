using System;
using UnityEngine;

public class IllusoryDoor : MonoBehaviour
{
    [SerializeField] private float distToCheck = 10f;
    private Collider[] _colliders = default;

    private void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        Transform p = GameManager.Instance.LevelManager.Player.transform;
        if (!(Vector3.Distance(p.position, transform.position) < distToCheck)) return;

        bool b = Vector3.Dot(p.forward, p.position - transform.position) > 0;
        foreach (var c in _colliders)
        {
            c.isTrigger = b;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}