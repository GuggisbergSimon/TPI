using System;
using UnityEngine;

public class IllusoryDoor : MonoBehaviour
{
    [SerializeField] private float distToCheck = 10f;
    [SerializeField, Range(0f, 90f)] private float angleDiffMax = 90f;
    private Collider[] _colliders = default;

    private void Start()
    {
        _colliders = GetComponentsInChildren<Collider>();
    }

    private void Update()
    {
        Transform p = GameManager.Instance.LevelManager.Player.transform;
        if (!(Vector3.Distance(p.position, transform.position) < distToCheck)) return;

        bool b = Vector3.Dot(p.forward, Vector3.ProjectOnPlane(p.position - transform.position, Vector3.up)) >
                 Mathf.Cos(angleDiffMax);
        foreach (var c in _colliders)
        {
            c.isTrigger = b;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position + Vector3.up * 0.5f, Vector3.one);
    }
}