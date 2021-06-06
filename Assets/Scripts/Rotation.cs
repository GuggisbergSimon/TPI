/*
 * Author : Simon Guggisberg
 * Date : 06.06.2021
 * Location : ETML
 * Description : Class rotating a transform around its y-axis
 */

using UnityEngine;

/// <summary>
/// Class rotating a transform around its y-axis
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