using UnityEngine;

/// <summary>
/// Moves the object on a sine wave
/// </summary>
public class Sine : MonoBehaviour
{
    [SerializeField, Tooltip("amplitude of the sine wave")]
    private float amplitude = 1f;
    [SerializeField, Min(StaticsValues.SMALLEST_POSITIVE_FLOAT), Tooltip("period of the sine wave")]
    private float period = 1f;
    [SerializeField, Tooltip("phase shift of the sine wave")]
    private float phaseShift = 0f;
    private Vector3 _initPos;

    private void Awake()
    {
        _initPos = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition =
            _initPos + transform.up * (amplitude * Mathf.Sin(Time.time * Mathf.PI * 2 / period + phaseShift));
    }
}