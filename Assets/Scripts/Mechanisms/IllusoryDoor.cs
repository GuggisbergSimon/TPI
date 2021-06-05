using UnityEngine;

/// <summary>
/// Class handling Illusory Doors behaviour; only letting player go through by walking backwards
/// </summary>
public class IllusoryDoor : MonoBehaviour
{
    [SerializeField] private float distToCheck = 10f;
    [SerializeField, Range(0f, 90f)] private float angleDiffMax = 90f;
    [SerializeField, Tooltip("Toggles SpriteRenderer children as well as colliders")] private bool enableSpriteBehaviour = false;
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
        //if player leave range, toggle the door back on
        if (_isOpen && !isCloseEnough)
        {
            Toggle(false);
        }

        //Toggles the door if the player is within the proper distance, depending on where they look
        if (!isCloseEnough) return;
        Vector3 illusoryToPlayer = player.position - transform.position;
        illusoryToPlayer = Vector3.ProjectOnPlane(illusoryToPlayer, Vector3.up);
        float fwdDirection = Vector3.Dot(transform.forward, illusoryToPlayer) > 0 ? 1f : -1f;
        Toggle(Vector3.Dot(player.forward, fwdDirection * transform.forward) > 0f);
    }

    /// <summary>
    /// Toggles the illusory door
    /// </summary>
    /// <param name="letsGoThrough">Wether it lets go through, or not</param>
    private void Toggle(bool letsGoThrough)
    {
        _isOpen = letsGoThrough;
        //sets the colliders as trigger to let the player go through
        foreach (var collider1 in _colliders)
        {
            collider1.isTrigger = letsGoThrough;
        }

        //disable sprites contained in the illusory door, can be disabled since it can be visible from the corner of the screen
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