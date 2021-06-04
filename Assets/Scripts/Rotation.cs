using UnityEngine;

/// <summary>
/// Rotates a transform around its y-axis
/// </summary>
public class Rotation : MonoBehaviour
{
    //todo add different axis
    [SerializeField, Tooltip("angle in degrees/seconds")] 
    private float speed = 1f;

    private void Update()
    {
        transform.Rotate(transform.up * (speed * Time.deltaTime), Space.World);
    }
}