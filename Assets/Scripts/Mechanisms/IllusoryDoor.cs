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
        Transform player = GameManager.Instance.LevelManager.Player.transform;
        bool isCloseEnough = Vector3.Distance(player.position, transform.position) < distToCheck;
        if (_isOpen && !isCloseEnough)
        {
            Toggle(false);
        }

        if (!isCloseEnough) return;

        Vector3 illusoryToPlayer = player.position - transform.position;
        illusoryToPlayer = Vector3.ProjectOnPlane(illusoryToPlayer, Vector3.up);
        Toggle(Vector3.Dot(player.forward,illusoryToPlayer) > Mathf.Cos(angleDiffMax));
    }

    private void Toggle(bool letsGoThrough)
    {
        _isOpen = letsGoThrough;
        foreach (var collider1 in _colliders)
        {
            collider1.isTrigger = letsGoThrough;
        }

        if (enableSpriteBehaviour)
        {
            foreach (var s in _sprites)
            {
                s.gameObject.SetActive(!letsGoThrough);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawCube(transform.position + Vector3.up * 0.5f, Vector3.one);
    }
}